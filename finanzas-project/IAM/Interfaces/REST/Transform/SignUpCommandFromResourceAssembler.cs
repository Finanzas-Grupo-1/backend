using finanzas_project.IAM.Domain.Model.Commands;
using finanzas_project.IAM.Interfaces.REST.Resources;

namespace finanzas_project.IAM.Interfaces.REST.Transform
{
    public static class SignUpCommandFromResourceAssembler
    {
        public static SignUpCommand ToCommandFromResource(SignUpResource resource)
        {
            return new SignUpCommand(resource.Username, resource.Password, resource.Role.ToUpper());
        }

    }
}
