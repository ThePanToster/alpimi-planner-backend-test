﻿using AlpimiAPI.Database;
using AlpimiAPI.Entities.EClassroomType;
using AlpimiAPI.Entities.EGroup;
using AlpimiAPI.Entities.ELesson;
using AlpimiAPI.Entities.ELesson.Commands;
using AlpimiAPI.Entities.ELessonType;
using AlpimiAPI.Entities.ESubgroup;
using AlpimiAPI.Locales;
using AlpimiAPI.Responses;
using AlpimiTest.TestSetup;
using AlpimiTest.TestUtilities;
using Microsoft.Extensions.Localization;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace AlpimiTest.Entities.ELesson.Commands
{
    [Collection("Sequential Tests")]
    public class CreateLessonCommandUnit
    {
        private readonly Mock<IDbService> _dbService = new();
        private readonly Mock<IStringLocalizer<Errors>> _str;

        public CreateLessonCommandUnit()
        {
            _str = ResourceSetup.Setup();
        }

        [Fact]
        public async Task ThrowsErrorWhenWrongLessonTypeIdIsGiven()
        {
            _dbService
                .Setup(s => s.Get<Subgroup>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetSubgroupDetails());

            var createLessonCommand = new CreateLessonCommand(
                new Guid(),
                MockData.GetCreateLessonDTODetails(new Guid(), new Guid()),
                new Guid(),
                "User"
            );
            var createLessonHandler = new CreateLessonHandler(_dbService.Object, _str.Object);
            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createLessonHandler.Handle(createLessonCommand, new CancellationToken())
            );

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[]
                    {
                        new ErrorObject(
                            "LessonType with id 00000000-0000-0000-0000-000000000000 was not found"
                        )
                    }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenWrongSubgroupIdIsGiven()
        {
            _dbService
                .Setup(s => s.Get<LessonType>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetLessonTypeDetails());

            var createLessonCommand = new CreateLessonCommand(
                new Guid(),
                MockData.GetCreateLessonDTODetails(new Guid(), new Guid()),
                new Guid(),
                "User"
            );
            var createLessonHandler = new CreateLessonHandler(_dbService.Object, _str.Object);
            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createLessonHandler.Handle(createLessonCommand, new CancellationToken())
            );

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[]
                    {
                        new ErrorObject(
                            "Subgroup with id 00000000-0000-0000-0000-000000000000 was not found"
                        )
                    }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenWrongClassroomTypeIdIsGiven()
        {
            var dto = MockData.GetCreateLessonDTODetails(new Guid(), new Guid());
            dto.ClassroomTypeIds = [new Guid()];
            _dbService
                .Setup(s => s.Get<LessonType>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetLessonTypeDetails());
            _dbService
                .Setup(s => s.Get<Subgroup>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetSubgroupDetails());
            _dbService
                .Setup(s => s.Get<Group>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetGroupDetails());

            var createLessonCommand = new CreateLessonCommand(new Guid(), dto, new Guid(), "User");
            var createLessonHandler = new CreateLessonHandler(_dbService.Object, _str.Object);
            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createLessonHandler.Handle(createLessonCommand, new CancellationToken())
            );

            Assert.Equal(
                "ClassroomType with id 00000000-0000-0000-0000-000000000000 was not found",
                result.errors.First().message
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenNameIsAlreadyTakenByLesson()
        {
            var dto = MockData.GetCreateLessonDTODetails(new Guid(), new Guid());
            _dbService
                .Setup(s => s.Get<LessonType>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetLessonTypeDetails());
            _dbService
                .Setup(s => s.Get<Subgroup>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetSubgroupDetails());
            _dbService
                .Setup(s => s.GetAll<Lesson>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(new List<Lesson> { MockData.GetLessonDetails() });
            _dbService
                .Setup(s => s.Get<Group>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetGroupDetails());

            var createLessonCommand = new CreateLessonCommand(new Guid(), dto, new Guid(), "User");
            var createLessonHandler = new CreateLessonHandler(_dbService.Object, _str.Object);
            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createLessonHandler.Handle(createLessonCommand, new CancellationToken())
            );

            Assert.Equal(
                "There is already a Lesson with the name Niski poziom",
                result.errors.First().message
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenAmountOfHoursIsLessThan1()
        {
            var dto = MockData.GetCreateLessonDTODetails(new Guid(), new Guid());
            dto.AmountOfHours = -1;

            var createLessonCommand = new CreateLessonCommand(new Guid(), dto, new Guid(), "User");
            var createLessonHandler = new CreateLessonHandler(_dbService.Object, _str.Object);
            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createLessonHandler.Handle(createLessonCommand, new CancellationToken())
            );

            Assert.Equal("AmountOfHours parameter is invalid", result.errors.First().message);
        }

        [Fact]
        public async Task ThrowsErrorWhenDuplicatedClassroomTypeIdsAreGiven()
        {
            var dto = MockData.GetCreateLessonDTODetails(new Guid(), new Guid());
            dto.ClassroomTypeIds = [new Guid(), new Guid()];
            _dbService
                .Setup(s => s.Get<LessonType>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetLessonTypeDetails());
            _dbService
                .Setup(s => s.Get<Subgroup>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetSubgroupDetails());
            _dbService
                .Setup(s => s.Get<Group>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetGroupDetails());

            var createLessonCommand = new CreateLessonCommand(new Guid(), dto, new Guid(), "User");
            var createLessonHandler = new CreateLessonHandler(_dbService.Object, _str.Object);
            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createLessonHandler.Handle(createLessonCommand, new CancellationToken())
            );

            Assert.Equal(
                "Cannot add multiple ClassroomType with the value 00000000-0000-0000-0000-000000000000",
                result.errors.First().message
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenScheduleIdsFromClassroomTypeAndLessonDontMatch()
        {
            var dto = MockData.GetCreateLessonDTODetails(new Guid(), new Guid());
            dto.ClassroomTypeIds = [new Guid()];
            var classroomType = MockData.GetClassroomTypeDetails();
            classroomType.ScheduleId = Guid.NewGuid();
            _dbService
                .Setup(s => s.Get<LessonType>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetLessonTypeDetails());
            _dbService
                .Setup(s => s.Get<Subgroup>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetSubgroupDetails());
            _dbService
                .Setup(s => s.Get<Group>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetGroupDetails());
            _dbService
                .Setup(s => s.Get<ClassroomType>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(classroomType);

            var createLessonCommand = new CreateLessonCommand(new Guid(), dto, new Guid(), "User");
            var createLessonHandler = new CreateLessonHandler(_dbService.Object, _str.Object);
            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createLessonHandler.Handle(createLessonCommand, new CancellationToken())
            );

            Assert.Equal(
                "ClassroomType must be in the same Schedule as Lesson",
                result.errors.First().message
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenScheduleIdsFromLessonTypeAndSubgroupDontMatch()
        {
            var dto = MockData.GetCreateLessonDTODetails(new Guid(), new Guid());
            var lessonType = MockData.GetLessonTypeDetails();
            lessonType.ScheduleId = Guid.NewGuid();
            _dbService
                .Setup(s => s.Get<LessonType>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(lessonType);
            _dbService
                .Setup(s => s.Get<Subgroup>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetSubgroupDetails());
            _dbService
                .Setup(s => s.Get<Group>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(MockData.GetGroupDetails());

            var createLessonCommand = new CreateLessonCommand(new Guid(), dto, new Guid(), "User");
            var createLessonHandler = new CreateLessonHandler(_dbService.Object, _str.Object);
            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createLessonHandler.Handle(createLessonCommand, new CancellationToken())
            );

            Assert.Equal(
                "Subgroup must be in the same Schedule as LessonType",
                result.errors.First().message
            );
        }
    }
}
