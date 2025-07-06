using finanzas_project.BonusesManagement.Domain.Model.Aggregates;
using finanzas_project.Shared.Domain.Repositories;

namespace finanzas_project.BonusesManagement.Domain.Repositories
{
    public interface IBonusesRepository : IBaseRepository<Bond>
    {

        Task<IEnumerable<Bond>> FindByUserIdAsync(int userId);


    }
}
