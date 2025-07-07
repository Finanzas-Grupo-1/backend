using finanzas_project.BonusesManagement.Domain.Model.Aggregates;
using finanzas_project.BonusesManagement.Domain.Model.Commands;
using finanzas_project.BonusesManagement.Domain.Repositories;
using finanzas_project.BonusesManagement.Domain.Services;
using finanzas_project.Shared.Domain.Repositories;

namespace finanzas_project.BonusesManagement.Application.Internal.CommandServices
{
    public class BondCommandService(IBonusesRepository bonusesRepository, IUnitOfWork unitOfWork, IBondValuation bondValuation) : IBondCommandService
    {
        public async Task<Bond?> Handle(CreateBondCommand command)
        {
            
            try
            {
                var bond = new Bond(command);

                var results = bondValuation.CalculateAll(bond);

                //Asignar resultados al bono
                bond.SetFinancialResults(
                    results.TCEA,
                    results.TREA,
                    results.Duration,
                    results.ModifiedDuration,
                    results.Convexity,
                    results.MaxPrice
                );

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

        public async Task<Bond?> Handle(UpdateBondCommand command)
        {
            var bond = await bonusesRepository.FindByIdAsync(command.BondId);
            if (bond == null) return null;

            bond.Update(
                command.Name, 
                command.NominalValue,
                command.CommercialValue,
                command.Years,
                command.PaymentsPerYear,
                command.CouponRate,
                command.RedemptionPremium,
                command.IsEffectiveRate,
                command.NominalRate,
                command.CapitalizationDays,
                command.Currency,
                command.StructuringCost,
                command.PlacementCost,
                command.FlotationCost,
                command.CavaliCost,
                command.TotalGracePeriods,
                command.PartialGracePeriods,
                command.MarketRate,
                command.CapitalizeInterests,
                command.StartDate
            );

            // Recalcular cash flows y métricas
            var results = bondValuation.CalculateAll(bond);
            bond.SetFinancialResults(
              results.TCEA,
              results.TREA,
              results.Duration,
              results.ModifiedDuration,
              results.Convexity,
              results.MaxPrice
            );

            await unitOfWork.CompleteAsync();
            return bond;
        }

        public async Task<bool> Handle(DeleteBondCommand command)
        {
            var bond = await bonusesRepository.FindByIdAsync(command.BondId);
            if (bond == null) return false;

            bonusesRepository.Remove(bond);
            await unitOfWork.CompleteAsync();
            return true;
        }
    }
}
