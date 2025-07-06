using finanzas_project.IAM.Application.Internal.OutboundServices;
using finanzas_project.IAM.Domain.Model.Aggregates;
using finanzas_project.IAM.Domain.Model.Commands;
using finanzas_project.IAM.Domain.Model.ValueObjects;
using finanzas_project.IAM.Domain.Repositories;
using finanzas_project.IAM.Domain.Services;
using finanzas_project.IAM.Infrastructure.Hashing.BCrypt.Services;
using finanzas_project.IAM.Infrastructure.Tokens.JWT.Services;
using finanzas_project.Shared.Domain.Repositories;
using System.Data;

namespace finanzas_project.IAM.Application.Internal.CommandServices
{
    public class UserCommandService(
     IUserRepository userRepository,
     IHashingService hashingService,
     ITokenService tokenService,
     IUnitOfWork unitOfWork
     ) : IUserCommandService
    {
        public async Task Handle(SignUpCommand command)
        {
            if (userRepository.ExistsByUsername(command.Username))
                throw new Exception($"Username {command.Username} is already taken");

            if (!Enum.TryParse<UserRole>(command.Role, true, out var role))
                throw new Exception($"Invalid role: {command.Role}");


            var hashedPassword = hashingService.HashPassword(command.Password);
            var user = new User(command.Username, hashedPassword).UpdateRole(role);
            try
            {
                await userRepository.AddAsync(user);
                await unitOfWork.CompleteAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred while creating user: {e.Message}");
            }
        }

        public async Task<(User user, string token)> Handle(SignInCommand command)
        {
            var user = await userRepository.FindByUsernameAsync(command.Username);
            if (user is null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
                throw new Exception("Invalid username or password");
            var token = tokenService.GenerateToken(user);
            return (user, token);
        }
    }
}
