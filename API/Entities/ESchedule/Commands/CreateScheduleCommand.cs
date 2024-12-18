﻿using AlpimiAPI.Database;
using AlpimiAPI.Entities.ESchedule.Queries;
using AlpimiAPI.Responses;
using alpimi_planner_backend.API.Locales;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AlpimiAPI.Entities.ESchedule.Commands
{
    public record CreateScheduleCommand(Guid Id, Guid UserId, string Name, int SchoolHour)
        : IRequest<Guid>;

    public class CreateScheduleHandler : IRequestHandler<CreateScheduleCommand, Guid>
    {
        private readonly IDbService _dbService;
        private readonly IStringLocalizer<Errors> _str;

        public CreateScheduleHandler(IDbService dbService, IStringLocalizer<Errors> str)
        {
            _dbService = dbService;
            _str = str;
        }

        public async Task<Guid> Handle(
            CreateScheduleCommand request,
            CancellationToken cancellationToken
        )
        {
            GetScheduleByNameHandler getScheduleByNameHandler = new GetScheduleByNameHandler(
                _dbService
            );
            GetScheduleByNameQuery getScheduleByNameQuery = new GetScheduleByNameQuery(
                request.Name,
                request.UserId,
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

            var insertedId = await _dbService.Post<Guid>(
                @"
                    INSERT INTO [Schedule] ([Id],[Name],[SchoolHour],[UserId])
                    OUTPUT INSERTED.Id                    
                    VALUES (@Id,@Name,@SchoolHour,@UserId);",
                request
            );

            return insertedId;
        }
    }
}
