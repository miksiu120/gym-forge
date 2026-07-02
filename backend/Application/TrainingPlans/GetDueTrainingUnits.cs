using WorkPlanner.Application.Abstractions;
using WorkPlanner.Application.Exceptions;
using WorkPlanner.Models;

namespace WorkPlanner.Application.TrainingPlans;

public sealed class GetDueTrainingUnits(
    ITrainingPlanRepository trainingPlans,
    ICurrentUser currentUser)
{
    public async Task<IReadOnlyList<DueTrainingUnitDto>> ExecuteAsync(
        DateTimeOffset through,
        CancellationToken cancellationToken = default)
    {
        if (through == default)
        {
            throw new BadRequestException("The through date is required.");
        }

        var units = await trainingPlans.GetDueUnitsAsync(
            currentUser.GetRequiredUserId(),
            through.UtcDateTime,
            cancellationToken);

        return units.Select(unit => new DueTrainingUnitDto
        {
            PlanId = unit.TrainingPlanId,
            PlanName = unit.TrainingPlan.Name,
            TrainingUnit = TrainingPlanMapper.ToUnitResponse(unit)
        }).ToList();
    }
}
