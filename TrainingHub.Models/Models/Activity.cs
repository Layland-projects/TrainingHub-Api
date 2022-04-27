using TrainingHub.Models.Models.Shared;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.Models.Models
{
    public class Activity : AuditableEntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public ActivityType Type { get; set; }
        public bool IsBodyWeight { get; set; }
    }
}
