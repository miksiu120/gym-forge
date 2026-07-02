using WorkPlanner.Application.Abstractions;
using WorkPlanner.Application.Exceptions;
using WorkPlanner.Models;

namespace WorkPlanner.Application.TrainingPlans;

public sealed class GetTrainingUnit(
    ITrainingPlanRepository trainingPlans,
    ICurrentUser currentUser)
{
    public async Task<TrainingUnitDto> ExecuteAsync(
        int unitId,
        CancellationToken cancellationToken = default)
    {
        var unit = await trainingPlans.GetUnitAsync(
            unitId,
            currentUser.GetRequiredUserId(),
            cancellationToken)
            ?? throw new NotFoundException("Training unit was not found.");

        return TrainingPlanMapper.ToUnitResponse(unit);
    }
}
