using finanzas_project.BonusesManagement.Domain.Model.Commands;
using finanzas_project.BonusesManagement.Domain.Model.Queries;
using finanzas_project.BonusesManagement.Domain.Services;
using finanzas_project.BonusesManagement.Interfaces.REST.Resources;
using finanzas_project.BonusesManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace finanzas_project.BonusesManagement.Interfaces.REST
{



    [ApiController]
    [Route("api/v1/[controller]")]
    public class BonusesController(IBondCommandService bondCommandService, IBondQueryService bondQueryService) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateBond([FromBody] CreateBondResource createBondResource)
        {
            var createBondCommand = CreateBondCommandFromResourceAssembler.ToCommandFromResource(createBondResource);
            var bond = await bondCommandService.Handle(createBondCommand);
            if (bond is null) return BadRequest();
            var resource = BondResourceFromEntityAssembler.ToResourceFromEntity(bond);
            return CreatedAtAction(nameof(GetBondById), new {bondId = resource.Id},resource);
        }


        [HttpGet("{bondId}")]
        public async Task<IActionResult> GetBondById([FromRoute] int bondId)
        {
            var bond =await bondQueryService.Handle(new GetBondByIdQuery(bondId));
            if(bond is null) return NotFound();
            var resource = BondResourceFromEntityAssembler.ToResourceFromEntity(bond);
            return Ok(resource);
        }



        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBondsByUserId([FromRoute] int userId)
        {
            var bonuses = await bondQueryService.Handle(new GetAllBonusesByUserIdQuery(userId));
            var resources = bonuses.Select(BondResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);

        }

        [HttpPut("{bondId}")]
        public async Task<IActionResult> UpdateBond([FromRoute] int bondId, [FromBody] UpdateBondResource updateBondResource)
        {
            var updateBondCommand = UpdateBondCommandFromResourceAssembler.ToCommandFromResource(bondId, updateBondResource);
            var bond = await bondCommandService.Handle(updateBondCommand);

            if (bond is null) return NotFound();

            var resource = BondResourceFromEntityAssembler.ToResourceFromEntity(bond);
            return Ok(resource);
        }


        [HttpDelete("{bondId}")]
        public async Task<IActionResult> DeleteBond([FromRoute] int bondId)
        {
            var command = new DeleteBondCommand(bondId);
            var success = await bondCommandService.Handle(command);
            if (!success) return NotFound();
            return NoContent();
        }



    }
}
