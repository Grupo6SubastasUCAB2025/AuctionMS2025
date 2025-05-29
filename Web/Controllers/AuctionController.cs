using Microsoft.AspNetCore.Mvc;
using MSAuction.Application.Commands;
using MSAuction.Application.DTOs;
using MediatR;

namespace MSAuction.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuctionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuction([FromBody] AuctionDto dto, [FromHeader] int userId)
        {
            var command = new CreateAuctionCommand(dto, userId);
            var auctionId = await _mediator.Send(command);
            return Ok(auctionId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAuction(int id, [FromBody] AuctionDto dto, [FromHeader] int userId)
        {
            var command = new UpdateAuctionCommand(id, dto, userId);
            var success = await _mediator.Send(command);

            if (success)
            {
                return Ok(new { message = "Subasta actualizada con éxito." });
            }
            else
            {
                return BadRequest(new { message = "No se pudo actualizar la subasta. Verifique los datos e intente nuevamente." });
            }
        }
    

    /*[HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuction(Guid id, [FromHeader] int userId)
    {
        var command = new DeleteAuctionCommand(id, userId);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuction(Guid id)
    {
        var auction = await _mediator.Send(new GetAuctionByIdQuery(id));
        return Ok(auction);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveAuctions()
    {
        var auctions = await _mediator.Send(new GetActiveAuctionsQuery());
        return Ok(auctions);
    }

    {
    "productId": 13,
    "title": "subasta",
    "description": "subasat2",
    "initialPrice": 10,
    "reservePrice": 110,
    "minIncrement": 10,
    "conditions": "ESo",
    "type": "normal",
    "startTime": "2025-05-27T11:35:27.385Z",
    "endTime": "2025-05-27T23:35:27.385Z"
    }*/
    }
}
