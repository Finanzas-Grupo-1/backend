using finanzas_project.BonusesManagement.Domain.Model.Aggregates;
using finanzas_project.BonusesManagement.Domain.Model.Entities;

namespace finanzas_project.BonusesManagement.Domain.Services
{
    public interface IBondValuation
    {

        BondValuationResult CalculateAll(Bond bond);

    }
}
