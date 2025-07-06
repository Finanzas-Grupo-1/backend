using finanzas_project.BonusesManagement.Domain.Model.Entities;
using finanzas_project.BonusesManagement.Domain.Model.Queries;

namespace finanzas_project.BonusesManagement.Domain.Services
{
    public interface ICashFlowQueryService
    {
        Task<IEnumerable<CashFlow>> Handle(GetAllCashFlowsByBondIdQuery query);

    }
}
