using WorkPlanner.Application.Abstractions;
using WorkPlanner.Models;

namespace WorkPlanner.Application.TrainingPlans;

public sealed class GetTrainingPlans(
    ITrainingPlanRepository trainingPlans,
    ICurrentUser currentUser)
{
    public async Task<IReadOnlyList<TrainingPlanDto>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        var plans = await trainingPlans.GetByAuthorAsync(
            currentUser.GetRequiredUserId(),
            cancellationToken);

        return plans.Select(TrainingPlanMapper.ToResponse).ToList();
    }
}
