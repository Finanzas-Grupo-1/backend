namespace finanzas_project.BonusesManagement.Domain.Model.Entities
{
    public class BondValuationResult
    {
        public decimal TCEA { get; set; }
        public decimal TREA { get; set; }
        public decimal Duration { get; set; }
        public decimal ModifiedDuration { get; set; }
        public decimal Convexity { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
