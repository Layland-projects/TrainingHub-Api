using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingHub.Models.Global;
using TrainingHub.Models.Models;

namespace TrainingHub.Infrastructure.Abstractions
{
    public interface ISessionService
    {
        public Task<Result<Session>> GetSession(int id, CancellationToken cancellationToken);
        public Task<Result<ICollection<Session>>> GetSessionsPaged(CancellationToken cancellationToken, int pageSize = 10, int pageNo = 0);
        public Task<Result<ICollection<Session>>> GetSessionsByUserId(int userId, CancellationToken cancellationToken);
        public Task<Result<ICollection<Session>>> GetSessionsByAzureUserId(Guid azureId, CancellationToken cancellationToken);
    }
}
