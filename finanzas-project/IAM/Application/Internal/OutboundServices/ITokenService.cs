using finanzas_project.IAM.Domain.Model.Aggregates;

namespace finanzas_project.IAM.Application.Internal.OutboundServices
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        Task<int?> ValidateToken(string token);
    }
}
