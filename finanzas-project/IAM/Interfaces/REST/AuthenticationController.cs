using finanzas_project.IAM.Domain.Services;
using finanzas_project.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using finanzas_project.IAM.Interfaces.REST.Resources;
using finanzas_project.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace finanzas_project.IAM.Interfaces.REST
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class AuthenticationController(IUserCommandService userCommandService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpResource resource)
        {
            var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
            await userCommandService.Handle(signUpCommand);
            return Ok(new { message = "User created successfully" });
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInResource resource)
        {
            var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
            var authenticatedUser = await userCommandService.Handle(signInCommand);
            var authenticatedUserResource =
                AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser.user,
                    authenticatedUser.token);
            return Ok(authenticatedUserResource);
        }
    }
}
