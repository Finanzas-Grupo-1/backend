using finanzas_project.IAM.Domain.Model.Aggregates;
using finanzas_project.IAM.Interfaces.REST.Resources;

namespace finanzas_project.IAM.Interfaces.REST.Transform
{
    public static class AuthenticatedUserResourceFromEntityAssembler
    {
        public static AuthenticatedUserResource ToResourceFromEntity(User entity, string token)
        {
            return new AuthenticatedUserResource(entity.Id, entity.Username, token);
        }
    }
}
