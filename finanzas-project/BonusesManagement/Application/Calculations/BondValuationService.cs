using finanzas_project.BonusesManagement.Domain.Model.Aggregates;
using finanzas_project.BonusesManagement.Domain.Model.Entities;
using finanzas_project.BonusesManagement.Domain.Services;

namespace finanzas_project.BonusesManagement.Application.Calculations
{
    public class BondValuationDomainService : IBondValuation
    {
        public BondValuationResult CalculateAll(Bond bond)
        {
            var cashFlows = bond.CashFlows.OrderBy(cf => cf.Period).ToList();
            decimal nominalValue = bond.NominalValue;
            decimal marketRate = bond.MarketRate / bond.PaymentsPerYear;

            var tcea = CalculateIRR(cashFlows, bond.CommercialValue, bond.PaymentsPerYear);
            var trea = CalculateIRR(cashFlows, nominalValue, bond.PaymentsPerYear);
            var duration = CalculateDuration(cashFlows, marketRate);
            var modifiedDuration = duration / (1 + marketRate);
            var convexity = CalculateConvexity(cashFlows, marketRate);
            var maxPrice = CalculateMaxPrice(cashFlows, marketRate);

            return new BondValuationResult
            {
                TCEA = tcea,
                TREA = trea,
                Duration = duration,
                ModifiedDuration = modifiedDuration,
                Convexity = convexity,
                MaxPrice = maxPrice
            };
        }

        private decimal CalculateIRR(List<CashFlow> cashFlows, decimal initialInvestment, int paymentsPerYear)
        {
            decimal guessRate = 0.1m;
            for (int i = 0; i < 1000; i++)
            {
                decimal npv = -initialInvestment;
                for (int t = 0; t < cashFlows.Count; t++)
                {
                    var cf = cashFlows[t];
                    npv += cf.TotalPayment / (decimal)Math.Pow(1 + (double)guessRate, cf.Period);
                }

                if (Math.Abs((double)npv) < 0.0001)
                    break;

                guessRate += 0.0001m;
            }

            return guessRate * paymentsPerYear;
        }

        private decimal CalculateDuration(List<CashFlow> cashFlows, decimal rate)
        {
            decimal duration = 0;
            decimal denominator = 0;

            foreach (var cf in cashFlows)
            {
                var discount = (decimal)Math.Pow(1 + (double)rate, cf.Period);
                duration += cf.TotalPayment * cf.Period / discount;
                denominator += cf.TotalPayment / discount;
            }
            return duration / denominator;
        }

        private decimal CalculateConvexity(List<CashFlow> cashFlows, decimal rate)
        {
            decimal convexity = 0;
            decimal denominator = 0;

            foreach (var cf in cashFlows)
            {
                var discount = (decimal)Math.Pow(1 + (double)rate, cf.Period);
                convexity += cf.TotalPayment * cf.Period * (cf.Period + 1) / (discount * discount);
                denominator += cf.TotalPayment / discount;
            }
            return convexity / (denominator * (1 + rate) * (1 + rate));
        }

        private decimal CalculateMaxPrice(List<CashFlow> cashFlows, decimal rate)
        {
            decimal price = 0;
            foreach (var cf in cashFlows)
            {
                price += cf.TotalPayment / (decimal)Math.Pow(1 + (double)rate, cf.Period);
            }
            return price;
        }
    }
}
