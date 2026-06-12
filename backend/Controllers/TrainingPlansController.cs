using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPlanner.Models;
using WorkPlanner.Services;

namespace WorkPlanner.Controllers;

[ApiController]
[Authorize]
[Route("api/training-plans")]
public sealed class TrainingPlansController : ControllerBase
{
    private readonly ITrainingPlanService _service;

    public TrainingPlansController(ITrainingPlanService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<TrainingPlanDto>>> GetMine() => Ok(await _service.GetMineAsync());

    [HttpGet("statistics")]
    public async Task<ActionResult<TrainingStatisticsDto>> GetStatistics() => Ok(await _service.GetStatisticsAsync());

    [HttpGet("due")]
    public async Task<ActionResult<List<DueTrainingUnitDto>>> GetDue([FromQuery] DateTimeOffset through) =>
        Ok(await _service.GetDueAsync(through));

    [HttpGet("units/{unitId:int}")]
    public async Task<ActionResult<TrainingUnitDto>> GetUnit(int unitId)
    {
        try
        {
            return Ok(await _service.GetUnitAsync(unitId));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpPut("units/{unitId:int}/complete")]
    public async Task<ActionResult<TrainingUnitDto>> CompleteUnit(
        int unitId,
        [FromBody] CompleteTrainingUnitDto request)
    {
        try
        {
            return Ok(await _service.CompleteUnitAsync(unitId, request));
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<TrainingPlanDto>> Create([FromBody] CreateTrainingPlanDto request)
    {
        try
        {
            var plan = await _service.CreateAsync(request);
            return Created($"api/training-plans/{plan.Id}", plan);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }
}
