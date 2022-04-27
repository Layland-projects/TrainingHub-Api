using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingHub.Models.Enums
{
    public class Enums
    {
        public enum ActivityType
        {
            Timed = 1,
            Repetitions = 2
        }

        public enum UserRole
        {
            Admin = 0,
            Trainer = 1,
            Trainee = 2,
        }

        public enum ResultStatus
        {
            NA = 0,
            Success = 1,
            Failed = 2,
            Error = 3,
        }
    }
}
