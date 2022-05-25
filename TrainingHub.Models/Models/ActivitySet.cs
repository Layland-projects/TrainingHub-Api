using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingHub.Models.Models.Shared;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.Models.Models
{
    public class ActivitySet : AuditableEntityBase
    {
        public Activity Activity { get; set; }
        public int ActivityId { get; set; }
        public int ActivityCount { get; set; }
        public int? DurationSeconds { get; set; }
        public decimal? WeightKgs { get; set; }
        public int? Repetitions { get; set; }
        public bool IsValid => DurationSeconds.HasValue && Activity.IsBodyWeight ||
            DurationSeconds.HasValue && WeightKgs.HasValue ||
            Repetitions.HasValue && WeightKgs.HasValue ||
            Repetitions.HasValue && Activity.IsBodyWeight;
    }
}
