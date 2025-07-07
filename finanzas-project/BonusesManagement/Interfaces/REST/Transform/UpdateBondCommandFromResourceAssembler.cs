using finanzas_project.BonusesManagement.Domain.Model.Commands;
using finanzas_project.BonusesManagement.Interfaces.REST.Resources;

namespace finanzas_project.BonusesManagement.Interfaces.REST.Transform
{
    public static class UpdateBondCommandFromResourceAssembler
    {
        public static UpdateBondCommand ToCommandFromResource(int bondId,UpdateBondResource resource)
        {
            return new UpdateBondCommand(
                bondId,
                resource.UserId,
                resource.Name,
                resource.NominalValue,
                resource.CommercialValue,
                resource.Years,
                resource.PaymentsPerYear,
                resource.CouponRate,
                resource.RedemptionPremium,
                resource.IsEffectiveRate,
                resource.NominalRate,
                resource.CapitalizationDays,
                resource.Currency,
                resource.StructuringCost,
                resource.PlacementCost,
                resource.FlotationCost,
                resource.CavaliCost,
                resource.TotalGracePeriods,
                resource.PartialGracePeriods,
                resource.MarketRate,
                resource.CapitalizeInterests,
                resource.StartDate
            );
        }
    }
}
