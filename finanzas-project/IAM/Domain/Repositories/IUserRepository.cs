using finanzas_project.IAM.Domain.Model.Aggregates;
using finanzas_project.Shared.Domain.Repositories;

namespace finanzas_project.IAM.Domain.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> FindByUsernameAsync(string username);
        bool ExistsByUsername(string username);
    }
}
