using finanzas_project.IAM.Domain.Model.Aggregates;
using finanzas_project.IAM.Domain.Repositories;
using finanzas_project.Shared.Infrastructure.Persistence.EFC.Configuration;
using finanzas_project.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace finanzas_project.IAM.Infrastructure.Persistence.EFC.Repositories
{
    public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task<User?> FindByUsernameAsync(string username)
        {
            return await Context.Set<User>().FirstOrDefaultAsync(user => user.Username.Equals(username));
        }

        public bool ExistsByUsername(string username)
        {
            return Context.Set<User>().Any(user => user.Username.Equals(username));
        }
    }
}
