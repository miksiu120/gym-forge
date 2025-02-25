using System.Text.Json.Serialization;

namespace WorkPlanner.Enums
{
    public enum ExerciseType
    {
        [JsonPropertyName("repetitive")]
        Repetitive = 0,
        [JsonPropertyName("timed")]
        Timed = 1
    }
}
