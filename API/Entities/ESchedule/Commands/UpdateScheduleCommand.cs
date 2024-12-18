﻿using AlpimiAPI.Database;
using AlpimiAPI.Entities.ESchedule.Queries;
using AlpimiAPI.Entities.EUser;
using AlpimiAPI.Entities.EUser.Queries;
using AlpimiAPI.Responses;
using alpimi_planner_backend.API.Locales;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AlpimiAPI.Entities.ESchedule.Commands
{
    public record UpdateScheduleCommand(
        Guid Id,
        string? Name,
        int? SchoolHour,
        Guid FilteredId,
        string Role
    ) : IRequest<Schedule?>;

    public class UpdateScheduleHandler : IRequestHandler<UpdateScheduleCommand, Schedule?>
    {
        private readonly IDbService _dbService;
        private readonly IStringLocalizer<Errors> _str;

        public UpdateScheduleHandler(IDbService dbService, IStringLocalizer<Errors> str)
        {
            _dbService = dbService;
            _str = str;
        }

        public async Task<Schedule?> Handle(
            UpdateScheduleCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Name != null)
            {
                GetScheduleByNameHandler getScheduleByNameHandler = new GetScheduleByNameHandler(
                    _dbService
                );
                GetScheduleByNameQuery getScheduleByNameQuery = new GetScheduleByNameQuery(
                    request.Name,
                    request.FilteredId,
                    "User"
                );
                ActionResult<Schedule?> scheduleName = await getScheduleByNameHandler.Handle(
                    getScheduleByNameQuery,
                    cancellationToken
                );

                if (scheduleName.Value != null)
                {
                    throw new ApiErrorException(
                        [new ErrorObject(_str["alreadyExists", "Schedule", request.Name])]
                    );
                }
            }

            Schedule? schedule;
            switch (request.Role)
            {
                case "Admin":
                    schedule = await _dbService.Update<Schedule?>(
                        @"
                    UPDATE [Schedule] 
                    SET [Name]=CASE WHEN @Name IS NOT NULL THEN @Name 
                    ELSE [Name] END,[SchoolHour]=CASE WHEN @SchoolHour IS NOT NULL THEN @SchoolHour ELSE [SchoolHour] END 
                    OUTPUT INSERTED.[Id], INSERTED.[Name], INSERTED.[SchoolHour]
                    WHERE [Id]=@Id;",
                        request
                    );
                    break;
                default:
                    schedule = await _dbService.Update<Schedule?>(
                        @"
                     UPDATE [Schedule] 
                    SET [Name]=CASE WHEN @Name IS NOT NULL THEN @Name 
                    ELSE [Name] END,[SchoolHour]=CASE WHEN @SchoolHour IS NOT NULL THEN @SchoolHour ELSE [SchoolHour] END 
                    OUTPUT INSERTED.[Id], INSERTED.[Name], INSERTED.[SchoolHour]
                    WHERE [Id]=@Id and [UserId]=@FilteredId;",
                        request
                    );
                    break;
            }

            if (schedule != null)
            {
                GetUserHandler getUserHandler = new GetUserHandler(_dbService);
                GetUserQuery getUserQuery = new GetUserQuery(schedule.UserId, new Guid(), "Admin");
                ActionResult<User?> user = await getUserHandler.Handle(
                    getUserQuery,
                    cancellationToken
                );
                schedule.User = user.Value!;
            }
            return schedule;
        }
    }
}
