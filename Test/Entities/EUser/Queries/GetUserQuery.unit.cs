﻿using AlpimiAPI.Database;
using AlpimiAPI.Entities.EUser;
using AlpimiAPI.Entities.EUser.Queries;
using Moq;
using Xunit;

namespace AlpimiTest.Entities.WUser.Queres
{
    public class GetUserCommandUnit
    {
        private readonly Mock<IDbService> _dbService = new();

        private User GetUserDetails()
        {
            var user = new User()
            {
                Id = new Guid(),
                Login = "marek",
                CustomURL = "44f"
            };

            return user;
        }

        [Fact]
        public async Task GetsUserWhenIdIsCorrect()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Get<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);

            var getUserCommand = new GetUserQuery(user.Id, new Guid(), "Admin");

            var getUserHandler = new GetUserHandler(_dbService.Object);

            var result = await getUserHandler.Handle(getUserCommand, new CancellationToken());

            Assert.Equal(user, result);
        }

        [Fact]
        public async Task ReturnsNullWhenIdIsIncorrect()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Get<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync((User?)null);

            var getUserCommand = new GetUserQuery(new Guid(), new Guid(), "Admin");

            var getUserHandler = new GetUserHandler(_dbService.Object);

            var result = await getUserHandler.Handle(getUserCommand, new CancellationToken());

            Assert.Null(result);
        }

        [Fact]
        public async Task ReturnsNullWhenWrongUserGetsDetails()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Get<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync((User?)null);

            var getUserCommand = new GetUserQuery(user.Id, new Guid(), "User");

            var getUserHandler = new GetUserHandler(_dbService.Object);

            var result = await getUserHandler.Handle(getUserCommand, new CancellationToken());

            Assert.Null(result);
        }
    }
}
