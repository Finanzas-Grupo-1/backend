using finanzas_project.BonusesManagement.Domain.Model.Entities;
using finanzas_project.Shared.Domain.Repositories;

namespace finanzas_project.BonusesManagement.Domain.Repositories
{
    public interface ICashFlowsRepository : IBaseRepository<CashFlow>
    {
        Task<IEnumerable<CashFlow>> FindByBondIdAsync(int BondId);
    }
}
