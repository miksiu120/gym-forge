using WorkPlanner.Application.Abstractions;
using WorkPlanner.Models;

namespace WorkPlanner.Application.TrainingPlans;

public sealed class GetTrainingStatistics(
    ITrainingPlanRepository trainingPlans,
    ICurrentUser currentUser)
{
    public async Task<TrainingStatisticsDto> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        var units = await trainingPlans.GetCompletedUnitsAsync(
            currentUser.GetRequiredUserId(),
            cancellationToken);

        return TrainingStatisticsCalculator.Calculate(units);
    }
}
