using finanzas_project.IAM.Domain.Model.Aggregates;
using finanzas_project.IAM.Domain.Model.Commands;

namespace finanzas_project.IAM.Domain.Services
{
    public interface IUserCommandService
    {
        Task Handle(SignUpCommand command);
        Task<(User user, string token)> Handle(SignInCommand command);
    }
}
