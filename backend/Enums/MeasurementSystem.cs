using System.Text.Json.Serialization;

namespace WorkPlanner.Enums
{
    public enum MeasurementSystem
    {
        [JsonPropertyName("metric")]
        Metric = 0,
        [JsonPropertyName("imperial")]
        Imperial = 1
    }


}
