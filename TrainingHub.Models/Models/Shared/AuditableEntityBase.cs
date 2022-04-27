using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingHub.Models.Models.Shared
{
    public abstract class AuditableEntityBase : EntityBase
    {
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
