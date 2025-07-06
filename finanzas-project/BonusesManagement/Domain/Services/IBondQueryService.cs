using finanzas_project.BonusesManagement.Domain.Model.Aggregates;
using finanzas_project.BonusesManagement.Domain.Model.Queries;

namespace finanzas_project.BonusesManagement.Domain.Services
{
    public interface IBondQueryService
    {
        Task<IEnumerable<Bond>> Handle(GetAllBonusesByUserIdQuery query);
        Task<IEnumerable<Bond>> Handle(GetAllBonusesQuery query);

        Task<Bond?> Handle(GetBondByIdQuery query); 

    }
}
