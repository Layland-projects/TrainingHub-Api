using TrainingHub.Models.Global;
using TrainingHub.Models.Models;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.Infrastructure.Abstractions
{
    public interface IUserService
    {
        static User? SignedInUser { get; private set; }
        Task<Result<User>> GetUserAsync(int id, CancellationToken cancellationToken);
        Task<Result<User>> GetUserByAzureIdAsync(Guid azureId, CancellationToken cancellationToken);
        Task<Result<IEnumerable<User>>> GetUsersAsync(int pageSize, int pageNum, CancellationToken cancellationToken);
        Task<Result> Register(Guid azureId, string contactNumber, string email, string firstName, string lastName, int role, 
            CancellationToken cancellationToken);
    }
}
