using finanzas_project.IAM.Domain.Model.Commands;
using finanzas_project.IAM.Interfaces.REST.Resources;

namespace finanzas_project.IAM.Interfaces.REST.Transform
{
    public static class SignInCommandFromResourceAssembler
    {
        public static SignInCommand ToCommandFromResource(SignInResource resource)
        {
            return new SignInCommand(resource.Username, resource.Password);
        }
    }
}
