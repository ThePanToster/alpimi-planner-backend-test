﻿using AlpimiAPI.Database;
using AlpimiAPI.Entities.ETeacher.Queries;
using AlpimiAPI.Locales;
using AlpimiAPI.Responses;
using AlpimiTest.TestSetup;
using Microsoft.Extensions.Localization;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace AlpimiTest.Entities.ETeacher.Queries
{
    [Collection("Sequential Tests")]
    public class GetAllTeachersByScheduleQueryUnit
    {
        private readonly Mock<IDbService> _dbService = new();
        private readonly Mock<IStringLocalizer<Errors>> _str;

        public GetAllTeachersByScheduleQueryUnit()
        {
            _str = ResourceSetup.Setup();
        }

        [Fact]
        public async Task ThrowsErrorWhenIncorrectPerPageIsGiven()
        {
            var getAllTeacherByScheduleQuery = new GetAllTeachersByScheduleQuery(
                new Guid(),
                new Guid(),
                "Admin",
                new PaginationParams(-20, 0, "Id", "ASC")
            );
            var getAllTeacherByScheduleHandler = new GetAllTeachersByScheduleHandler(
                _dbService.Object,
                _str.Object
            );

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await getAllTeacherByScheduleHandler.Handle(
                        getAllTeacherByScheduleQuery,
                        new CancellationToken()
                    )
            );

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[] { new ErrorObject("PerPage parameter is invalid") }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenIncorrectPageIsGiven()
        {
            var getAllTeacherByScheduleQuery = new GetAllTeachersByScheduleQuery(
                new Guid(),
                new Guid(),
                "Admin",
                new PaginationParams(20, -1, "Id", "ASC")
            );
            var getAllTeacherByScheduleHandler = new GetAllTeachersByScheduleHandler(
                _dbService.Object,
                _str.Object
            );

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await getAllTeacherByScheduleHandler.Handle(
                        getAllTeacherByScheduleQuery,
                        new CancellationToken()
                    )
            );

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[] { new ErrorObject("Page parameter is invalid") }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenIncorrectSortByIsGiven()
        {
            var getAllTeacherByScheduleQuery = new GetAllTeachersByScheduleQuery(
                new Guid(),
                new Guid(),
                "Admin",
                new PaginationParams(20, 0, "wrong", "ASC")
            );
            var getAllTeacherByScheduleHandler = new GetAllTeachersByScheduleHandler(
                _dbService.Object,
                _str.Object
            );

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await getAllTeacherByScheduleHandler.Handle(
                        getAllTeacherByScheduleQuery,
                        new CancellationToken()
                    )
            );

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[] { new ErrorObject("SortBy parameter is invalid") }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenIncorrectSortOrderIsGiven()
        {
            var getAllTeacherByScheduleQuery = new GetAllTeachersByScheduleQuery(
                new Guid(),
                new Guid(),
                "Admin",
                new PaginationParams(20, 0, "Id", "wrong")
            );
            var getAllTeacherByScheduleHandler = new GetAllTeachersByScheduleHandler(
                _dbService.Object,
                _str.Object
            );

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await getAllTeacherByScheduleHandler.Handle(
                        getAllTeacherByScheduleQuery,
                        new CancellationToken()
                    )
            );

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[] { new ErrorObject("SortOrder parameter is invalid") }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsMultipleErrorMessages()
        {
            var getAllTeacherByScheduleQuery = new GetAllTeachersByScheduleQuery(
                new Guid(),
                new Guid(),
                "Admin",
                new PaginationParams(20, 0, "wrong", "wrong")
            );
            var getAllTeacherByScheduleHandler = new GetAllTeachersByScheduleHandler(
                _dbService.Object,
                _str.Object
            );

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await getAllTeacherByScheduleHandler.Handle(
                        getAllTeacherByScheduleQuery,
                        new CancellationToken()
                    )
            );

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[]
                    {
                        new ErrorObject("SortOrder parameter is invalid"),
                        new ErrorObject("SortBy parameter is invalid")
                    }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }
    }
}
