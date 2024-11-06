﻿using AlpimiAPI.Database;
using AlpimiAPI.Entities.EUser;
using AlpimiAPI.Entities.EUser.Commands;
using AlpimiAPI.Responses;
using AlpimiAPI.Settings;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace AlpimiTest.Entities.EUser.Commands
{
    public class CreateUserCommandUnit
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
        public async Task CreatesUserWhenPaswordIsCorrect()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Post<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);

            var createUserCommand = new CreateUserCommand(
                user.Id,
                new Guid(),
                user.Login,
                user.CustomURL!,
                "RandomPassword!1"
            );

            var createUserHandler = new CreateUserHandler(_dbService.Object);

            var result = await createUserHandler.Handle(createUserCommand, new CancellationToken());

            Assert.Equal(user.Id, result);
        }

        [Fact]
        public async Task ThrowsErrorWhenPasswordIsTooShort()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Post<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);

            var createUserCommand = new CreateUserCommand(
                user.Id,
                new Guid(),
                user.Login,
                user.CustomURL!,
                "Rand1!"
            );

            var createUserHandler = new CreateUserHandler(_dbService.Object);

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createUserHandler.Handle(createUserCommand, new CancellationToken())
            );

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[]
                    {
                        new ErrorObject(
                            "Password cannot be shorter than "
                                + AuthSettings.MinimumPasswordLength
                                + " characters"
                        )
                    }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenPasswordIsTooLong()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Post<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);

            var createUserCommand = new CreateUserCommand(
                user.Id,
                new Guid(),
                user.Login,
                user.CustomURL!,
                "RandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandomRandom1!"
            );

            var createUserHandler = new CreateUserHandler(_dbService.Object);

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createUserHandler.Handle(createUserCommand, new CancellationToken())
            );

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[]
                    {
                        new ErrorObject(
                            "Password cannot be longer than "
                                + AuthSettings.MaximumPasswordLength
                                + " characters"
                        )
                    }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenPasswordDosentContainSmallLetters()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Post<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);

            var createUserCommand = new CreateUserCommand(
                user.Id,
                new Guid(),
                user.Login,
                user.CustomURL!,
                "RANDOMBIG1!"
            );

            var createUserHandler = new CreateUserHandler(_dbService.Object);

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createUserHandler.Handle(createUserCommand, new CancellationToken())
            );
            var requiredCharacters = AuthSettings.RequiredCharacters;

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[]
                    {
                        new ErrorObject(
                            "Password must contain at least one of the following: "
                                + string.Join(", ", requiredCharacters!)
                        )
                    }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenPasswordDosentContainBigLetters()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Post<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);

            var createUserCommand = new CreateUserCommand(
                user.Id,
                new Guid(),
                user.Login,
                user.CustomURL!,
                "randomsmall1!"
            );

            var createUserHandler = new CreateUserHandler(_dbService.Object);

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createUserHandler.Handle(createUserCommand, new CancellationToken())
            );
            var requiredCharacters = AuthSettings.RequiredCharacters;

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[]
                    {
                        new ErrorObject(
                            "Password must contain at least one of the following: "
                                + string.Join(", ", requiredCharacters!)
                        )
                    }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenPasswordDosentContainSymbols()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Post<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);

            var createUserCommand = new CreateUserCommand(
                user.Id,
                new Guid(),
                user.Login,
                user.CustomURL!,
                "Randomsmall1"
            );

            var createUserHandler = new CreateUserHandler(_dbService.Object);

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createUserHandler.Handle(createUserCommand, new CancellationToken())
            );
            var requiredCharacters = AuthSettings.RequiredCharacters;

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[]
                    {
                        new ErrorObject(
                            "Password must contain at least one of the following: "
                                + string.Join(", ", requiredCharacters!)
                        )
                    }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenPasswordDosentContainDigits()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Post<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);

            var createUserCommand = new CreateUserCommand(
                user.Id,
                new Guid(),
                user.Login,
                user.CustomURL!,
                "Randomsmall!"
            );

            var createUserHandler = new CreateUserHandler(_dbService.Object);

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createUserHandler.Handle(createUserCommand, new CancellationToken())
            );
            var requiredCharacters = AuthSettings.RequiredCharacters;

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[]
                    {
                        new ErrorObject(
                            "Password must contain at least one of the following: "
                                + string.Join(", ", requiredCharacters!)
                        )
                    }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenLoginAlreadyExists()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Post<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);
            _dbService
                .Setup(s => s.Get<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);

            var createUserCommand = new CreateUserCommand(
                user.Id,
                new Guid(),
                user.Login,
                user.CustomURL!,
                "Randomsmall1!"
            );

            var createUserHandler = new CreateUserHandler(_dbService.Object);

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createUserHandler.Handle(createUserCommand, new CancellationToken())
            );
            var requiredCharacters = AuthSettings.RequiredCharacters;

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[] { new ErrorObject("Login already taken") }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsErrorWhenURLAlreadyExists()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Post<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);
            _dbService
                .Setup(s => s.Get<string>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user.CustomURL);
            var createUserCommand = new CreateUserCommand(
                user.Id,
                new Guid(),
                user.Login,
                user.CustomURL!,
                "Randomsmall1!"
            );

            var createUserHandler = new CreateUserHandler(_dbService.Object);

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createUserHandler.Handle(createUserCommand, new CancellationToken())
            );

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[] { new ErrorObject("URL already taken") }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }

        [Fact]
        public async Task ThrowsMultipleErrorMessages()
        {
            var user = GetUserDetails();

            _dbService
                .Setup(s => s.Post<User>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(user);

            var createUserCommand = new CreateUserCommand(
                user.Id,
                new Guid(),
                user.Login,
                user.CustomURL!,
                "R1!"
            );

            var createUserHandler = new CreateUserHandler(_dbService.Object);

            var result = await Assert.ThrowsAsync<ApiErrorException>(
                async () =>
                    await createUserHandler.Handle(createUserCommand, new CancellationToken())
            );
            var requiredCharacters = AuthSettings.RequiredCharacters;

            Assert.Equal(
                JsonConvert.SerializeObject(
                    new ErrorObject[]
                    {
                        new ErrorObject(
                            "Password cannot be shorter than "
                                + AuthSettings.MinimumPasswordLength
                                + " characters"
                        ),
                        new ErrorObject(
                            "Password must contain at least one of the following: "
                                + string.Join(", ", requiredCharacters!)
                        )
                    }
                ),
                JsonConvert.SerializeObject(result.errors)
            );
        }
    }
}
