using finanzas_project.IAM.Domain.Model.Queries;
using finanzas_project.IAM.Domain.Services;
using finanzas_project.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using finanzas_project.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace finanzas_project.IAM.Interfaces.REST
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsersController(IUserQueryService userQueryService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var getAllUsersQuery = new GetAllUsersQuery();
            var users = await userQueryService.Handle(getAllUsersQuery);
            var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(userResources);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var getUserByIdQuery = new GetUserByIdQuery(id);
            var user = await userQueryService.Handle(getUserByIdQuery);
            if (user is null) return NotFound();
            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return Ok(userResource);
        }
    }
}
