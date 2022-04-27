using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingHub.Models.Models.Shared;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.Models.Models
{
    public class User : AuditableEntityBase
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string ContactNumber { get; set; } //could make this a type but is it worth it?
        public string Email { get; set; } //could make this a type but is it worth it?
        public int AuthId { get; set; } //this is the .net auth user id

        public UserRole Role { get; set; }

        public int? TrainerId { get; set; }
        public User? Trainer { get; set; }
        public ICollection<Session>? Sessions { get; set; } = new List<Session>();
        public ICollection<User>? Trainees { get; set; } = new List<User>();
    }
}
