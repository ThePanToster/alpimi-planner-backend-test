﻿using System.Security.Cryptography;
using AlpimiAPI.Database;
using AlpimiAPI.Entities.EUser.Queries;
using AlpimiAPI.Responses;
using AlpimiAPI.Settings;
using AlpimiAPI.Utilities;
using alpimi_planner_backend.API.Locales;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AlpimiAPI.Entities.EUser.Commands
{
    public record CreateUserCommand(
        Guid Id,
        Guid AuthId,
        string Login,
        string CustomURL,
        string Password
    ) : IRequest<Guid>;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IDbService _dbService;
        private readonly IStringLocalizer<Errors> _str;

        public CreateUserHandler(IDbService dbService, IStringLocalizer<Errors> str)
        {
            _dbService = dbService;
            _str = str;
        }

        public async Task<Guid> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken
        )
        {
            GetUserByLoginHandler getUserByLoginHandler = new GetUserByLoginHandler(_dbService);
            GetUserByLoginQuery getUserByLoginQuery = new GetUserByLoginQuery(
                request.Login,
                new Guid(),
                "Admin"
            );
            ActionResult<User?> user = await getUserByLoginHandler.Handle(
                getUserByLoginQuery,
                cancellationToken
            );

            List<ErrorObject> errors = new List<ErrorObject>();
            if (user.Value != null)
            {
                errors.Add(new ErrorObject(_str["alreadyExists", "User", request.Login]));
            }

            var userURL = await _dbService.Get<string>(
                @"SELECT [CustomURL]
                FROM [User]
                WHERE [CustomURL] = @CustomURL;",
                request
            );

            if (userURL != null)
            {
                errors.Add(new ErrorObject(_str["alreadyExists", "URL", request.CustomURL]));
            }
            if (request.Password.Length < AuthSettings.MinimumPasswordLength)
            {
                errors.Add(
                    new ErrorObject(_str["shortPassword", AuthSettings.MinimumPasswordLength])
                );
            }

            if (request.Password.Length > AuthSettings.MaximumPasswordLength)
            {
                errors.Add(
                    new ErrorObject(_str["longPassword", AuthSettings.MaximumPasswordLength])
                );
            }
            RequiredCharacterTypes[]? requiredCharacterTypes = AuthSettings.RequiredCharacters;
            bool requiredCharactersError = false;

            if (requiredCharacterTypes != null)
            {
                if (requiredCharacterTypes.Contains(RequiredCharacterTypes.BigLetter))
                {
                    if (!request.Password.Any(char.IsUpper))
                    {
                        requiredCharactersError = true;
                    }
                }
                if (requiredCharacterTypes.Contains(RequiredCharacterTypes.SmallLetter))
                {
                    if (!request.Password.Any(char.IsLower))
                    {
                        requiredCharactersError = true;
                    }
                }
                if (requiredCharacterTypes.Contains(RequiredCharacterTypes.Digit))
                {
                    if (!request.Password.Any(char.IsDigit))
                    {
                        requiredCharactersError = true;
                    }
                }
                if (requiredCharacterTypes.Contains(RequiredCharacterTypes.Symbol))
                {
                    if (
                        !(
                            request.Password.Any(char.IsSymbol)
                            || request.Password.Any(char.IsPunctuation)
                        )
                    )
                    {
                        requiredCharactersError = true;
                    }
                }
            }

            if (requiredCharactersError)
            {
                errors.Add(
                    new ErrorObject(
                        _str["passwordMustContain", string.Join(", ", requiredCharacterTypes!)]
                    )
                );
            }

            if (errors.Count != 0)
            {
                throw new ApiErrorException(errors);
            }

            var insertedId = await _dbService.Post<Guid>(
                @"
                    INSERT INTO [User] ([Id],[Login],[CustomURL])
                    OUTPUT INSERTED.Id                    
                    VALUES (@Id,@Login,@CustomURL);",
                request
            );
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                request.Password,
                salt,
                Configuration.GetHashIterations(),
                Configuration.GetHashAlgorithm(),
                Configuration.GetKeySize()
            );

            await _dbService.Post<Guid>(
                @"
                    INSERT INTO [Auth] ([Id],[Password],[Salt],[Role],[UserId])
                    OUTPUT INSERTED.UserId                    
                    VALUES (@AuthId,'"
                    + Convert.ToBase64String(hash)
                    + "','"
                    + Convert.ToBase64String(salt)
                    + "','"
                    + "User"
                    + "',@Id);",
                request
            );

            return insertedId;
        }
    }
}
