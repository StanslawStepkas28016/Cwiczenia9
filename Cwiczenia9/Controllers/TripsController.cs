using Cwiczenia9.ModelsDtos;
using Cwiczenia9.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private ITripsService _tripsService;

    public TripsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    [HttpGet("GetTripsDto")]
    public async Task<IActionResult> GetTripsDto(bool paging, CancellationToken cancellationToken, int pageNum = 1,
        int pageSize = 10)
    {
        var res = await _tripsService.GetTripsDto(paging, cancellationToken, pageNum, pageSize);

        if (!res.Any())
        {
            return BadRequest("The Database contains no data!");
        }

        return Ok(res);
    }

    [HttpDelete("DeleteClient")]
    public async Task<IActionResult> DeleteClient(int clientId, CancellationToken cancellationToken)
    {
        var res = await _tripsService.DeleteClient(clientId, cancellationToken);

        if (res == -1)
        {
            return BadRequest("The Client has assigned trips!");
        }

        return Ok(res);
    }

    [HttpPut("AssignClient")]
    public async Task<IActionResult> AssignClientToTrip(ClientTripDto clientTripDto,
        CancellationToken cancellationToken)
    {
        var res = await _tripsService.AssignClientToTrip(clientTripDto, cancellationToken);

        return GetResponseForIntCode(res);
    }

    private IActionResult GetResponseForIntCode(int code)
    {
        var responses = new Dictionary<int, IActionResult>()
        {
            { 1, Ok("Client successfully assigned!") },
            { -1, BadRequest("Client with the provided Pesel does not exist in the database!") },
            { -2, BadRequest("Trip with the provided Id does not exist in the database!") },
            { -3, BadRequest("Provided trip DateFrom for trip is not in the future!") },
            { -4, BadRequest("Client with the provided Pesel is already assigned to the provided Trip (IdTrip)!") }
        };

        return responses[code];
    }
}