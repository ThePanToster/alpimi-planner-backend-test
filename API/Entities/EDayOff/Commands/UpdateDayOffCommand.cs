﻿using AlpimiAPI.Database;
using AlpimiAPI.Entities.EDayOff.DTO;
using AlpimiAPI.Entities.EScheduleSettings;
using AlpimiAPI.Entities.EScheduleSettings.Queries;
using AlpimiAPI.Locales;
using AlpimiAPI.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AlpimiAPI.Entities.EDayOff.Commands
{
    public record UpdateDayOffCommand(Guid Id, UpdateDayOffDTO dto, Guid FilteredId, string Role)
        : IRequest<DayOff?>;

    public class UpdateDayOffHandler : IRequestHandler<UpdateDayOffCommand, DayOff?>
    {
        private readonly IDbService _dbService;
        private readonly IStringLocalizer<Errors> _str;

        public UpdateDayOffHandler(IDbService dbService, IStringLocalizer<Errors> str)
        {
            _dbService = dbService;
            _str = str;
        }

        public async Task<DayOff?> Handle(
            UpdateDayOffCommand request,
            CancellationToken cancellationToken
        )
        {
            var scheduleSettings = await _dbService.Get<ScheduleSettings?>(
                @"
                    SELECT
                    ss.[Id], [SchoolHour], [SchoolYearStart], [SchoolYearEnd], [ScheduleId]
                    FROM [ScheduleSettings] ss
                    INNER JOIN [DayOff] do ON do.[ScheduleSettingsId]=ss.[Id]
                    WHERE do.[Id]=@Id ;",
                request
            );

            if (scheduleSettings == null)
            {
                return null;
            }

            var originalDayOff = await _dbService.Get<DayOff?>(
                @"
                    SELECT 
                    [Id],[Name],[From],[To],[ScheduleSettingsId]
                    FROM [DayOff] 
                    WHERE [Id]=@Id;",
                request
            );

            if ((request.dto.From ?? originalDayOff!.From) > (request.dto.To ?? originalDayOff!.To))
            {
                throw new ApiErrorException([new ErrorObject(_str["scheduleDate"])]);
            }
            if (
                (request.dto.From ?? originalDayOff!.From) < scheduleSettings.SchoolYearStart
                || (request.dto.To ?? originalDayOff!.To) > scheduleSettings.SchoolYearEnd
            )
            {
                throw new ApiErrorException(
                    [
                        new ErrorObject(
                            _str[
                                "dateOutOfRange",
                                scheduleSettings.SchoolYearStart.ToString("dd/MM/yyyy"),
                                scheduleSettings.SchoolYearEnd.ToString("dd/MM/yyyy")
                            ]
                        )
                    ]
                );
            }

            DayOff? dayOff;
            switch (request.Role)
            {
                case "Admin":
                    dayOff = await _dbService.Update<DayOff?>(
                        $@"
                            UPDATE [DayOff] 
                            SET 
                            [Name]=CASE WHEN @Name IS NOT NULL THEN @Name ELSE [Name] END,
                            [From]=CASE WHEN @From IS NOT NULL THEN @From ELSE [From] END,
                            [To]=CASE WHEN @To IS NOT NULL THEN @To ELSE [To] END
                            OUTPUT
                            INSERTED.[Id], 
                            INSERTED.[Name],
                            INSERTED.[From],
                            INSERTED.[To],
                            INSERTED.[ScheduleSettingsId]
                            WHERE [Id] = '{request.Id}';",
                        request.dto
                    );
                    break;
                default:
                    dayOff = await _dbService.Update<DayOff?>(
                        $@"
                            UPDATE do
                            SET 
                            [Name] = CASE WHEN @Name IS NOT NULL THEN @Name ELSE do.[Name] END,
                            [From]=CASE WHEN @From IS NOT NULL THEN @From ELSE [From] END,
                            [To]=CASE WHEN @To IS NOT NULL THEN @To ELSE [To] END
                            OUTPUT 
                            INSERTED.[Id], 
                            INSERTED.[Name], 
                            INSERTED.[From], 
                            INSERTED.[To],
                            INSERTED.[ScheduleSettingsId]
                            FROM [DayOff] do
                            INNER JOIN [ScheduleSettings] ss ON ss.[Id] = do.[ScheduleSettingsId]
                            INNER JOIN [Schedule] s ON s.[Id] = ss.[ScheduleId]
                            WHERE s.[UserId] = '{request.FilteredId}' AND do.[Id] = '{request.Id}';",
                        request.dto
                    );
                    break;
            }

            if (dayOff != null)
            {
                GetScheduleSettingsHandler getScheduleSettingsHandler =
                    new GetScheduleSettingsHandler(_dbService);
                GetScheduleSettingsQuery getScheduleSettingsQuery = new GetScheduleSettingsQuery(
                    dayOff.ScheduleSettingsId,
                    new Guid(),
                    "Admin"
                );
                ActionResult<ScheduleSettings?> toInsertScheduleSettings =
                    await getScheduleSettingsHandler.Handle(
                        getScheduleSettingsQuery,
                        cancellationToken
                    );
                dayOff.ScheduleSettings = toInsertScheduleSettings.Value!;
            }

            return dayOff;
        }
    }
}
