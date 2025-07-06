using finanzas_project.IAM.Domain.Model.Commands;
using finanzas_project.IAM.Domain.Model.Queries;
using finanzas_project.IAM.Domain.Services;

namespace finanzas_project.IAM.Interfaces.ACL.Services
{
    public class IamContextFacade(
     IUserQueryService userQueryService,
     IUserCommandService userCommandService) : IIamContextFacade
    {
        public async Task<int> CreateUser(string username, string password,string role)
        {
            var signUpCommand = new SignUpCommand(username, password,role);
            await userCommandService.Handle(signUpCommand);
            var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
            var result = await userQueryService.Handle(getUserByUsernameQuery);
            return result?.Id ?? 0;
        }

        public async Task<int> FetchUserIdByUsername(string username)
        {
            var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
            var result = await userQueryService.Handle(getUserByUsernameQuery);
            return result?.Id ?? 0;
        }

        public async Task<string> FetchUsernameByUserId(int userId)
        {
            var getUserByIdQuery = new GetUserByIdQuery(userId);
            var result = await userQueryService.Handle(getUserByIdQuery);
            return result?.Username ?? string.Empty;
        }
    }
}
