using finanzas_project.BonusesManagement.Domain.Model.Aggregates;
using finanzas_project.BonusesManagement.Domain.Model.Queries;
using finanzas_project.BonusesManagement.Domain.Repositories;
using finanzas_project.BonusesManagement.Domain.Services;

namespace finanzas_project.BonusesManagement.Application.Internal.QueryServices
{
    public class BondQueryService(IBonusesRepository bonusesRepository) : IBondQueryService
    {
        public async Task<IEnumerable<Bond>> Handle(GetAllBonusesByUserIdQuery query)
        {
            return await bonusesRepository.FindByUserIdAsync(query.UserId);
        }

        public async Task<IEnumerable<Bond>> Handle(GetAllBonusesQuery query)
        {
            return await bonusesRepository.ListAsync();
        }

        public async Task<Bond?> Handle(GetBondByIdQuery query)
        {
            return await bonusesRepository.FindByIdAsync(query.Id);
        }
    }
}
