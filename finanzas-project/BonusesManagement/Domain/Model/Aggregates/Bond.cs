using finanzas_project.BonusesManagement.Domain.Model.Commands;
using finanzas_project.BonusesManagement.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace finanzas_project.BonusesManagement.Domain.Model.Aggregates
{
    public class Bond
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        // Parámetros del bono
        public decimal NominalValue { get; private set; }
        public decimal CommercialValue { get; private set; }
        public int Years { get; private set; }
        public int PaymentsPerYear { get; private set; }
        public decimal CouponRate { get; private set; }
        public decimal RedemptionPremium { get; private set; }
        public bool IsEffectiveRate { get; private set; }
        public decimal? NominalRate { get; private set; }
        public int? CapitalizationDays { get; private set; }
        public string Currency { get; private set; }


        // Costos asociados
        public decimal StructuringCost { get; private set; }
        public decimal PlacementCost { get; private set; }
        public decimal FlotationCost { get; private set; }
        public decimal CavaliCost { get; private set; }


        // Periodos de gracia
        public int TotalGracePeriods { get; private set; }
        public int PartialGracePeriods { get; private set; }


        // Tasa de interés de mercado
        public decimal MarketRate { get; private set; }


        // Resultados financieros (output)
        public decimal? TCEA { get; private set; }
        public decimal? TREA { get; private set; }
        public decimal? Duration { get; private set; }
        public decimal? ModifiedDuration { get; private set; }
        public decimal? Convexity { get; private set; }
        public decimal? MaxPrice { get; private set; }

        public DateTime StartDate { get; private set; }

        [Column("CreatedAt")] public DateTimeOffset? CreatedDate { get; set; }
        [Column("UpdatedAt")] public DateTimeOffset? UpdatedDate { get; set; }

        // Relación
        public List<CashFlow> CashFlows { get; private set; } = new();


        public Bond() { }



        public Bond(int userId, string name, decimal nominalValue, decimal commercialValue, int years,
           int paymentsPerYear, decimal couponRate, decimal redemptionPremium, bool isEffectiveRate,
           decimal? nominalRate, int? capitalizationDays, string currency,
           decimal structuringCost, decimal placementCost, decimal flotationCost, decimal cavaliCost,
           int totalGracePeriods, int partialGracePeriods, decimal marketRate, DateTime startDate) : this()
        {
           
            UserId = userId;
            Name = name;
            NominalValue = nominalValue;
            CommercialValue = commercialValue;
            Years = years;
            PaymentsPerYear = paymentsPerYear;
            CouponRate = couponRate;
            RedemptionPremium = redemptionPremium;
            IsEffectiveRate = isEffectiveRate;
            NominalRate = nominalRate;
            CapitalizationDays = capitalizationDays;
            Currency = currency;
            StructuringCost = structuringCost;
            PlacementCost = placementCost;
            FlotationCost = flotationCost;
            CavaliCost = cavaliCost;
            TotalGracePeriods = totalGracePeriods;
            PartialGracePeriods = partialGracePeriods;
            MarketRate = marketRate;
            StartDate = startDate;
           
        }

        //  Genera el flujo de caja automáticamente
        public void GenerateCashFlows(DateTime startDate, bool capitalizarIntereses = false)
        {
            var totalPeriods = Years * PaymentsPerYear;
            var monthsBetweenPayments = 12 / PaymentsPerYear;
            decimal capitalActual = NominalValue;

            for (int period = 1; period <= totalPeriods; period++)
            {
                var paymentDate = startDate.AddMonths(period * monthsBetweenPayments);
                decimal interest = 0;
                decimal amortization = 0;

                if (period <= TotalGracePeriods)
                {
                    interest = capitalActual * CouponRate / PaymentsPerYear;

                    if (capitalizarIntereses)
                        capitalActual += interest;

                    amortization = 0;
                }
                else if (period <= TotalGracePeriods + PartialGracePeriods)
                {
                    interest = capitalActual * CouponRate / PaymentsPerYear;
                    amortization = 0;
                }
                else if (period < totalPeriods)
                {
                    interest = capitalActual * CouponRate / PaymentsPerYear;
                    amortization = 0;
                }
                else
                {
                    interest = capitalActual * CouponRate / PaymentsPerYear;
                    amortization = capitalActual * (1 + RedemptionPremium);
                }

                var remainingDebt = (period == totalPeriods) ? 0 : capitalActual;

                var cashFlow = new CashFlow(
                    bondId: Id,
                    period: period,
                    paymentDate: paymentDate,
                    interest: Math.Round(interest, 2),
                    amortization: Math.Round(amortization, 2),
                    remainingDebt: Math.Round(remainingDebt, 2)
                );

                CashFlows.Add(cashFlow);
            }
        }

        // Establece los resultados financieros calculados
        public void SetFinancialResults(decimal tcea, decimal trea, decimal duration, decimal modDuration, decimal convexity, decimal maxPrice)
        {
            TCEA = tcea;
            TREA = trea;
            Duration = duration;
            ModifiedDuration = modDuration;
            Convexity = convexity;
            MaxPrice = maxPrice;
        }

        public Bond(CreateBondCommand command)
        {
            UserId = command.UserId;
            Name = command.Name;
            NominalValue = command.NominalValue;
            CommercialValue = command.CommercialValue;
            Years = command.Years;
            PaymentsPerYear = command.PaymentsPerYear;
            CouponRate = command.CouponRate;
            RedemptionPremium = command.RedemptionPremium;
            IsEffectiveRate = command.IsEffectiveRate;
            NominalRate = command.NominalRate;
            CapitalizationDays = command.CapitalizationDays;
            Currency = command.Currency;
            StructuringCost = command.StructuringCost;
            PlacementCost = command.PlacementCost;
            FlotationCost = command.FlotationCost;
            CavaliCost = command.CavaliCost;
            TotalGracePeriods = command.TotalGracePeriods;
            PartialGracePeriods = command.PartialGracePeriods;
            MarketRate = command.MarketRate;
            StartDate = command.StartDate;

            //  Se genera el flujo automáticamente desde el command
            GenerateCashFlows(command.StartDate, command.CapitalizeInterests);
        }


    }
}
