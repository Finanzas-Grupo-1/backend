using finanzas_project.BonusesManagement.Domain.Model.Aggregates;
using finanzas_project.BonusesManagement.Domain.Model.Commands;

namespace finanzas_project.BonusesManagement.Domain.Services
{
    public interface IBondCommandService
    {

        Task<Bond?> Handle(CreateBondCommand command);


    }
}
