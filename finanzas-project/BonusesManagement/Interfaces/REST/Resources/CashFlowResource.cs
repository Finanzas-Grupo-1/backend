namespace finanzas_project.BonusesManagement.Interfaces.REST.Resources
{
    public record CashFlowResource(int Period,
        DateTime PaymentDate,
        decimal Interest,
        decimal Amortization,
        decimal TotalPayment,
        decimal RemainingDebt);
}
