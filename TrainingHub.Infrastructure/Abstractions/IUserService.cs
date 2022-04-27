using TrainingHub.Models.Global;
using TrainingHub.Models.Models;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.Infrastructure.Abstractions
{
    public interface IUserService
    {
        static User? SignedInUser { get; private set; }
        Task<Result<User>> GetUserAsync(int id, CancellationToken cancellationToken);
        Task<Result<IEnumerable<User>>> GetUsersAsync(int pageSize, int pageNum, CancellationToken cancellationToken);
        Task<Result<User>> CreateUserAsync(string title, string firstName, string lastName, string contactNum, string email, string password, UserRole role);
        Task<Result> UpdatePasswordAsync(int id, string password);
        Task<Result> UpdateEmailAsync(int id, string email);
        Task<Result> UpdateUsernameAsync(int id, string username);
        Task<Result> UpdateUserAsync(int id, string title, string firstName, string lastName);
        Task<Result<int>> SignIn(string username, string password);
        Task<Result> SignOut(int id);
        Task<Result> ForgotPassword(string email);
    }
}
