using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingHub.Infrastructure.Abstractions
{
    public interface ITimestampService
    {
        DateTime GetDateTime();
        DateTime GetUTCDateTime();
        DateTimeOffset GetDateTimeOffset();
        DateTimeOffset GetUTCDateTimeOffset();
    }
}
