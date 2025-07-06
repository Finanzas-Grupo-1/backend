using finanzas_project.BonusesManagement.Domain.Model.Aggregates;
using finanzas_project.BonusesManagement.Domain.Model.Commands;
using finanzas_project.BonusesManagement.Domain.Repositories;
using finanzas_project.BonusesManagement.Domain.Services;
using finanzas_project.Shared.Domain.Repositories;

namespace finanzas_project.BonusesManagement.Application.Internal.CommandServices
{
    public class BondCommandService(IBonusesRepository bonusesRepository, IUnitOfWork unitOfWork) : IBondCommandService
    {
        public async Task<Bond?> Handle(CreateBondCommand command)
        {
            
            try
            {
                var bond = new Bond(command);
                await bonusesRepository.AddAsync(bond);
                await unitOfWork.CompleteAsync();
                return bond;
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while creating the bond: {e.Message}");
                return null;
            }



        }
    }
}
