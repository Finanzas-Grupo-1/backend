using finanzas_project.BonusesManagement.Domain.Model.Entities;
using finanzas_project.BonusesManagement.Interfaces.REST.Resources;

namespace finanzas_project.BonusesManagement.Interfaces.REST.Transform
{
    public static class CashFlowResourceFromEntityAssembler
    {
        public static CashFlowResource ToResourceFromEntity(CashFlow entity)
        {
            return new CashFlowResource(entity.Period,
                entity.PaymentDate, entity.Interest, entity.Amortization, entity.TotalPayment, entity.RemainingDebt);
        }


    }
}
