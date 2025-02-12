﻿using AlpimiAPI.Database;
using AlpimiAPI.Entities.ESchedule;
using AlpimiAPI.Entities.ESchedule.Queries;
using AlpimiAPI.Locales;
using AlpimiAPI.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AlpimiAPI.Entities.EClassroomType.Queries
{
    public record GetAllClassroomTypesQuery(
        Guid Id,
        Guid FilteredId,
        string Role,
        PaginationParams Pagination
    ) : IRequest<(IEnumerable<ClassroomType>?, int)>;

    public class GetAllClassroomTypesHandler
        : IRequestHandler<GetAllClassroomTypesQuery, (IEnumerable<ClassroomType>?, int)>
    {
        private readonly IDbService _dbService;
        private readonly IStringLocalizer<Errors> _str;

        public GetAllClassroomTypesHandler(IDbService dbService, IStringLocalizer<Errors> str)
        {
            _dbService = dbService;
            _str = str;
        }

        public async Task<(IEnumerable<ClassroomType>?, int)> Handle(
            GetAllClassroomTypesQuery request,
            CancellationToken cancellationToken
        )
        {
            List<ErrorObject> errors = new List<ErrorObject>();
            if (request.Pagination.PerPage < 0)
            {
                errors.Add(new ErrorObject(_str["badParameter", "PerPage"]));
            }
            if (request.Pagination.Offset < 0)
            {
                errors.Add(new ErrorObject(_str["badParameter", "Page"]));
            }
            if (
                request.Pagination.SortOrder.ToLower() != "asc"
                && request.Pagination.SortOrder.ToLower() != "desc"
            )
            {
                errors.Add(new ErrorObject(_str["badParameter", "SortOrder"]));
            }
            if (request.Pagination.SortBy != "Id" && request.Pagination.SortBy != "Name")
            {
                errors.Add(new ErrorObject(_str["badParameter", "SortBy"]));
            }

            if (errors.Count != 0)
            {
                throw new ApiErrorException(errors);
            }

            IEnumerable<ClassroomType>? classroomTypes;
            int count;
            switch (request.Role)
            {
                case "Admin":
                    count = await _dbService.Get<int>(
                        @"
                            SELECT 
                            COUNT(*)
                            FROM [ClassroomType] ct
                            LEFT JOIN [ClassroomClassroomType] cct ON cct.[ClassroomTypeId] = ct.[Id]
                            LEFT JOIN [Classroom] c ON c.[Id] = cct.[ClassroomId]
                            LEFT JOIN [LessonClassroomType] lct ON lct.[ClassroomTypeId] = ct.[Id]
                            LEFT JOIN [Lesson] l ON l.[Id] = lct.[LessonId]
                            WHERE ct.[ScheduleId] = @Id OR c.[Id] = @Id OR l.[Id] = @Id;",
                        request
                    );
                    classroomTypes = await _dbService.GetAll<ClassroomType>(
                        $@"
                            SELECT
                            ct.[Id], ct.[Name], ct.[ScheduleId] 
                            FROM [ClassroomType] ct
                            LEFT JOIN [ClassroomClassroomType] cct ON cct.[ClassroomTypeId] = ct.[Id]
                            LEFT JOIN [Classroom] c ON c.[Id] = cct.[ClassroomId]
                            LEFT JOIN [LessonClassroomType] lct ON lct.[ClassroomTypeId] = ct.[Id]
                            LEFT JOIN [Lesson] l ON l.[Id] = lct.[LessonId]
                            WHERE ct.[ScheduleId] = @Id OR c.[Id] = @Id OR l.[Id] = @Id
                            ORDER BY
                            {request.Pagination.SortBy}
                            {request.Pagination.SortOrder}
                            OFFSET
                            {request.Pagination.Offset} ROWS
                            FETCH NEXT
                            {request.Pagination.PerPage} ROWS ONLY;",
                        request
                    );
                    break;
                default:
                    count = await _dbService.Get<int>(
                        @"
                            SELECT
                            COUNT(*)
                            FROM [ClassroomType] ct
                            INNER JOIN [Schedule] s ON s.[Id] = ct.[ScheduleId]
                            LEFT JOIN [ClassroomClassroomType] cct ON cct.[ClassroomTypeId] = ct.[Id]
                            LEFT JOIN [Classroom] c ON c.[Id] = cct.[ClassroomId]
                            LEFT JOIN [LessonClassroomType] lct ON lct.[ClassroomTypeId] = ct.[Id]
                            LEFT JOIN [Lesson] l ON l.[Id] = lct.[LessonId]
                            WHERE s.[UserId] = @FilteredId AND (ct.[ScheduleId] = @Id OR c.[Id] = @Id OR l.[Id] = @Id);",
                        request
                    );
                    classroomTypes = await _dbService.GetAll<ClassroomType>(
                        $@"
                            SELECT 
                            ct.[Id], ct.[Name], ct.[ScheduleId] 
                            FROM [ClassroomType] ct
                            INNER JOIN [Schedule] s ON s.[Id] = ct.[ScheduleId]
                            LEFT JOIN [ClassroomClassroomType] cct ON cct.[ClassroomTypeId] = ct.[Id]
                            LEFT JOIN [Classroom] c ON c.[Id] = cct.[ClassroomId]
                            LEFT JOIN [LessonClassroomType] lct ON lct.[ClassroomTypeId] = ct.[Id]
                            LEFT JOIN [Lesson] l ON l.[Id] = lct.[LessonId]
                            WHERE s.[UserId] = @FilteredId AND (ct.[ScheduleId] = @Id OR c.[Id] = @Id OR l.[Id] = @Id)
                            ORDER BY
                            {request.Pagination.SortBy}
                            {request.Pagination.SortOrder}
                            OFFSET
                            {request.Pagination.Offset} ROWS
                            FETCH NEXT
                            {request.Pagination.PerPage} ROWS ONLY;",
                        request
                    );
                    break;
            }

            if (classroomTypes != null)
            {
                Dictionary<Guid, Schedule> scheduleMap = new Dictionary<Guid, Schedule>();
                foreach (var classroomType in classroomTypes)
                {
                    if (!scheduleMap.ContainsKey(classroomType.ScheduleId))
                    {
                        GetScheduleHandler getScheduleHandler = new GetScheduleHandler(_dbService);
                        GetScheduleQuery getScheduleQuery = new GetScheduleQuery(
                            classroomType.ScheduleId,
                            new Guid(),
                            "Admin"
                        );
                        ActionResult<Schedule?> schedule = await getScheduleHandler.Handle(
                            getScheduleQuery,
                            cancellationToken
                        );

                        scheduleMap.Add(classroomType.ScheduleId, schedule.Value!);
                    }
                    classroomType.Schedule = scheduleMap[classroomType.ScheduleId];
                }
            }

            return (classroomTypes, count);
        }
    }
}
