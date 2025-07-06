using finanzas_project.IAM.Domain.Model.Aggregates;
using finanzas_project.IAM.Domain.Model.Queries;

namespace finanzas_project.IAM.Domain.Services
{
    public interface IUserQueryService
    {
        Task<IEnumerable<User>> Handle(GetAllUsersQuery query);
        Task<User?> Handle(GetUserByIdQuery query);
        Task<User?> Handle(GetUserByRoleQuery query);
        Task<User?> Handle(GetUserByUsernameQuery query);

    }
}
