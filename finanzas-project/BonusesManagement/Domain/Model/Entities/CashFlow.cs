using finanzas_project.BonusesManagement.Domain.Model.Aggregates;

namespace finanzas_project.BonusesManagement.Domain.Model.Entities
{
    public class CashFlow
    {
        public int Id { get; private set; } 
        public int BondId { get;  set; }
        public Bond Bond { get;  set; }
        public int Period { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public decimal Interest { get; private set; }
        public decimal Amortization { get; private set; }
        public decimal TotalPayment => Interest + Amortization;
        public decimal RemainingDebt { get; private set; }

        protected CashFlow() { }

        public CashFlow(int bondId, int period, DateTime paymentDate,
            decimal interest, decimal amortization, decimal remainingDebt)
        {
            BondId = bondId;
            Period = period;
            PaymentDate = paymentDate;
            Interest = interest;
            Amortization = amortization;
            RemainingDebt = remainingDebt;
        }



    }
}
