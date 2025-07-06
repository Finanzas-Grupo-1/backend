using finanzas_project.BonusesManagement.Domain.Model.Aggregates;
using finanzas_project.BonusesManagement.Interfaces.REST.Resources;

namespace finanzas_project.BonusesManagement.Interfaces.REST.Transform
{
    public static class BondResourceFromEntityAssembler
    {
        public static BondResource ToResourceFromEntity(Bond b)
        {
            return new BondResource(
                 b.Id,
                 b.UserId,
                 b.Name,
                 b.NominalValue,
                 b.CommercialValue,
                 b.Years,
                 b.PaymentsPerYear,
                 b.CouponRate,
                 b.RedemptionPremium,
                 b.IsEffectiveRate,
                 b.NominalRate,
                 b.CapitalizationDays,
                 b.Currency,
                 b.StructuringCost,
                 b.PlacementCost,
                 b.FlotationCost,
                 b.CavaliCost,
                 b.TotalGracePeriods,
                 b.PartialGracePeriods,
                 b.MarketRate,
                 b.TCEA,
                 b.TREA,
                 b.Duration,
                 b.ModifiedDuration,
                 b.Convexity,
                 b.MaxPrice,
                 b.StartDate,
                  b.CashFlows
                    .Select(CashFlowResourceFromEntityAssembler.ToResourceFromEntity)
                    .ToList()
            );
        }



    }
}
