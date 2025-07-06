using finanzas_project.IAM.Domain.Model.Aggregates;
using finanzas_project.IAM.Domain.Model.Queries;
using finanzas_project.IAM.Domain.Repositories;
using finanzas_project.IAM.Domain.Services;

namespace finanzas_project.IAM.Application.Internal.QueryServices
{
    public class UserQueryService(IUserRepository userRepository) : IUserQueryService
    {
        public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
        {
            return await userRepository.ListAsync();
        }

        public async Task<User?> Handle(GetUserByIdQuery query)
        {
            return await userRepository.FindByIdAsync(query.Id);
        }


        public Task<User?> Handle(GetUserByRoleQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> Handle(GetUserByUsernameQuery query)
        {
            return await userRepository.FindByUsernameAsync(query.Username);
        }
    }
}
