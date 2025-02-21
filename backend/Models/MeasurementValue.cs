using System.Diagnostics.Metrics;
using WorkPlanner.Enums;

namespace WorkPlanner.Models
{
    public class Measure
    {
        public double Value { get; set; }
        public MeasurementSystem MeasurementSystem { get; set; }
    }
}
