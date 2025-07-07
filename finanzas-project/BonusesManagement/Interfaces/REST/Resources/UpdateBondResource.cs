namespace finanzas_project.BonusesManagement.Interfaces.REST.Resources
{
    public record UpdateBondResource(
        int UserId,
        string Name,
        decimal NominalValue,
        decimal CommercialValue,
        int Years,
        int PaymentsPerYear,
        decimal CouponRate,
        decimal RedemptionPremium,
        bool IsEffectiveRate,
        decimal? NominalRate,
        int? CapitalizationDays,
        string Currency,
        decimal StructuringCost,
        decimal PlacementCost,
        decimal FlotationCost,
        decimal CavaliCost,
        int TotalGracePeriods,
        int PartialGracePeriods,
        decimal MarketRate,
        bool CapitalizeInterests,
        DateTime StartDate
    );
}
