using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPlanner.Application.TrainingPlans;
using WorkPlanner.Models;

namespace WorkPlanner.Controllers;

[ApiController]
[Authorize]
[Route("api/training-plans")]
public sealed class TrainingPlansController(
    GetTrainingPlans getTrainingPlans,
    GetTrainingStatistics getTrainingStatistics,
    GetDueTrainingUnits getDueTrainingUnits,
    GetTrainingUnit getTrainingUnit,
    CompleteTrainingUnit completeTrainingUnit,
    CreateTrainingPlan createTrainingPlan) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TrainingPlanDto>>> GetMine(
        CancellationToken cancellationToken) =>
        Ok(await getTrainingPlans.ExecuteAsync(cancellationToken));

    [HttpGet("statistics")]
    public async Task<ActionResult<TrainingStatisticsDto>> GetStatistics(
        CancellationToken cancellationToken) =>
        Ok(await getTrainingStatistics.ExecuteAsync(cancellationToken));

    [HttpGet("due")]
    public async Task<ActionResult<IReadOnlyList<DueTrainingUnitDto>>> GetDue(
        [FromQuery] DateTimeOffset through,
        CancellationToken cancellationToken) =>
        Ok(await getDueTrainingUnits.ExecuteAsync(through, cancellationToken));

    [HttpGet("units/{unitId:int}")]
    public async Task<ActionResult<TrainingUnitDto>> GetUnit(
        int unitId,
        CancellationToken cancellationToken) =>
        Ok(await getTrainingUnit.ExecuteAsync(unitId, cancellationToken));

    [HttpPut("units/{unitId:int}/complete")]
    public async Task<ActionResult<TrainingUnitDto>> CompleteUnit(
        int unitId,
        [FromBody] CompleteTrainingUnitDto request,
        CancellationToken cancellationToken) =>
        Ok(await completeTrainingUnit.ExecuteAsync(unitId, request, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<TrainingPlanDto>> Create(
        [FromBody] CreateTrainingPlanDto request,
        CancellationToken cancellationToken)
    {
        var plan = await createTrainingPlan.ExecuteAsync(request, cancellationToken);
        return Created($"/api/training-plans/{plan.Id}", plan);
    }
}
