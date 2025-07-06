using finanzas_project.IAM.Domain.Model.Aggregates;
using finanzas_project.IAM.Interfaces.REST.Resources;

namespace finanzas_project.IAM.Interfaces.REST.Transform
{
    public static class UserResourceFromEntityAssembler
    {
        public static UserResource ToResourceFromEntity(User entity)
        {
            return new UserResource(entity.Id, entity.Username);
        }
    }
}
