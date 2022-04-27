using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingHub.Models.Models
{
    public class Session
    {
        public User Trainee { get; set; }
        public int TraineeId { get; set; }
        public User Trainer { get; set; }
        public int TrainerId { get; set; }
        public DateTimeOffset ScheduledFor { get; set; }
        public ICollection<ActivitySet> Sets { get; set; } = new List<ActivitySet>();
        public bool IsCompleted { get; set; }
    }
}
