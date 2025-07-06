using finanzas_project.BonusesManagement.Domain.Model.Aggregates;
using finanzas_project.BonusesManagement.Domain.Repositories;
using finanzas_project.Shared.Infrastructure.Persistence.EFC.Configuration;
using finanzas_project.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace finanzas_project.BonusesManagement.Infrastructure.Persistence.EFC
{
    public class BondRepository(AppDbContext context) : BaseRepository<Bond>(context), IBonusesRepository
    {
        public async Task<IEnumerable<Bond>> FindByUserIdAsync(int userId)
        {
            return await Context.Set<Bond>()
                .Include(b => b.CashFlows)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public new async Task<Bond?> FindByIdAsync(int id) => await Context.Set<Bond>()
             .Include(b => b.CashFlows)
            .FirstOrDefaultAsync(bond => bond.Id == id);


    }
}
