﻿using AlpimiAPI.Database;
using AlpimiAPI.Responses;
using alpimi_planner_backend.API.Locales;
using MediatR;
using Microsoft.Extensions.Localization;

namespace AlpimiAPI.Entities.EUser.Commands
{
    public record UpdateUserCommand(
        Guid Id,
        string? Login,
        string? CustomURL,
        Guid FilteredId,
        string Role
    ) : IRequest<User?>;

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, User?>
    {
        private readonly IDbService _dbService;
        private readonly IStringLocalizer<Errors> _str;

        public UpdateUserHandler(IDbService dbService, IStringLocalizer<Errors> str)
        {
            _dbService = dbService;
            _str = str;
        }

        public async Task<User?> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.CustomURL != null)
            {
                var userURL = await _dbService.Get<string>(
                    @"SELECT [CustomURL]
                FROM [User]
                WHERE [CustomURL] = @CustomURL;",
                    request
                );
                if (userURL != null)
                {
                    throw new ApiErrorException(
                        [new ErrorObject(_str["alreadyExists", "URL", request.CustomURL])]
                    );
                }
            }
            User? user;
            switch (request.Role)
            {
                case "Admin":
                    user = await _dbService.Update<User?>(
                        @"
                    UPDATE [User] 
                    SET [Login]=CASE WHEN @Login IS NOT NULL THEN @Login 
                    ELSE [Login] END,[CustomURL]=CASE WHEN @CustomURL IS NOT NULL THEN @CustomURL ELSE [CustomURL] END 
                    OUTPUT INSERTED.[Id], INSERTED.[Login], INSERTED.[CustomURL]
                    WHERE [Id]=@Id;",
                        request
                    );
                    break;
                default:
                    user = await _dbService.Update<User?>(
                        @"
                    UPDATE [User] 
                    SET [Login]=CASE WHEN @Login IS NOT NULL THEN @Login 
                    ELSE [Login] END,[CustomURL]=CASE WHEN @CustomURL IS NOT NULL THEN @CustomURL ELSE [CustomURL] END 
                    OUTPUT INSERTED.[Id], INSERTED.[Login], INSERTED.[CustomURL]
                    WHERE [Id]=@Id AND [Id] = @FilteredId;",
                        request
                    );
                    break;
            }
            return user;
        }
    }
}
