using TrainingHub.Infrastructure.Abstractions;
using TrainingHub.Models.Global;
using TrainingHub.Models.Models;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.Infrastructure.Implementations.Mock
{
    public class MockUserService : IUserService
    {
        public static User? SignedInUser { get; private set; }
        private static int currentId = 3;
        private static readonly List<User> mockUsers = new()
        {
            new User
            {
                Id = 1,
                AzureId = Guid.Parse("855106d5-6b44-4bfd-94c3-c86fc22d4e06"), 
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = "Systen",
                ContactNumber = "07862752792",
                Email = "laylandoo@gmail.com",
                Title = "Mr",
                FirstName = "Dan",
                LastName = "Layland",
                Role = UserRole.Admin,
                Sessions = new List<Session>(),
                Trainees = null,
                Trainer = null,
                TrainerId = null
            },
            new User
            {
                Id = 2,
                AzureId = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = "System",
                ContactNumber = "077061906060",
                Email = "trainer@test.com",
                Title = "Miss",
                FirstName = "Emma",
                LastName = "Barker",
                Role = UserRole.Trainer,
                Sessions = new List<Session>(),
                Trainees = null,
                Trainer = null,
                TrainerId = null,
            },
            new User
            {
                Id = 3,
                AzureId = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = "System",
                ContactNumber = "01234567890",
                Email = "trainee@test.com",
                Title = "Mr",
                FirstName = "Fresh",
                LastName = "Meat",
                Role = UserRole.Trainee,
                Sessions = new List<Session>(),
                Trainees = null,
                Trainer = null,
                TrainerId = 2
            }
        };
        private readonly ITimestampService timestampService;

        public MockUserService(ITimestampService timestampService)
        {
            this.timestampService = timestampService;
        }

        public Task<Result<User>> CreateUserAsync(string title, string firstName, string lastName, string contactNum, string email, string password, UserRole role)
        {
            var existingUser = mockUsers.SingleOrDefault(x => x.Email == email);
            // ToDo: change to return error saying user already registered
            if (existingUser != null)
            {
                return Task.FromResult(Result.FailureFrom<User>(null)) ;
            }
            var rand = new Random();
            var user = new User
            {
                Id = ++currentId,
                AzureId = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Title = title,
                Email = email,
                ContactNumber = contactNum,
                CreatedAt = timestampService.GetDateTimeOffset(),
                UpdatedAt = timestampService.GetDateTimeOffset(),
                Role = role,
                CreatedBy = email,
                UpdatedBy = email
            };
            mockUsers.Add(user);
            return Task.FromResult(Result.SuccessFrom(user));
        }

        public Task<Result<User>> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            var user = mockUsers.Where(x => x.Id == id).SingleOrDefault();
            if (user == null)
            {
                return Task.FromResult(Result.FailureFrom<User>(null, $"User /w id: {id} not found"));
            }
            return Task.FromResult(Result.SuccessFrom(user));
        }

        public Task<Result<IEnumerable<User>>> GetUsersAsync(int pageSize, int pageNum, CancellationToken cancellationToken)
        {
            var users = mockUsers.Skip(pageNum * pageSize).Take(pageSize);
            if (users == null)
            {
                return Task.FromResult(Result.FailureFrom(Enumerable.Empty<User>(),
                    $"No entries for pageNum: {pageNum}, pageSize: {pageSize}"));
            }
            return Task.FromResult(Result.SuccessFrom(users));
        }

        public Task<Result> Register(Guid azureId, string contactNumber, string email, string firstName, string lastName,
            int role, CancellationToken cancellationToken)
        {
            var user = mockUsers.SingleOrDefault(x => x.AzureId == azureId);
            if (user == null)
            {
                user = new()
                {
                    CreatedAt = timestampService.GetDateTimeOffset(),
                    CreatedBy = email,
                    AzureId = azureId,
                    Id = ++currentId,
                };
            }
            var uRole = (UserRole)role;
            user.UpdatedAt = timestampService.GetDateTimeOffset();
            user.UpdatedBy = email;
            user.ContactNumber = contactNumber;
            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Role = uRole;
            user.Title = string.IsNullOrEmpty(user.Title) ? "Mx" : user.Title;
            user.Sessions = user.Sessions!= null && user.Sessions.Count > 0 ? user.Sessions : new List<Session>();
            user.Trainees = uRole != UserRole.Trainee ? user.Trainees != null && user.Trainees.Count > 0 ? user.Trainees : new List<User>() : null;
            user.Trainer = null; //might be a way to find this out
            user.TrainerId = null; //might be a way to find this out
            user.Username = email;
            if (mockUsers.Any(x => x.AzureId == azureId))
            {
                mockUsers.Add(user);
            }
            return Task.FromResult(Result.Success());
        }

        public Task<Result<User>> GetUserByAzureIdAsync(Guid azureId, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<Result<User>>(cancellationToken);
            }
            var user = mockUsers.SingleOrDefault(x => x.AzureId == azureId);
            return user == null ? Task.FromResult(Result.FailureFrom<User>(null, $"No User exists with the provided Id: {azureId}"))
                : Task.FromResult(Result<User>.SuccessFrom(user));
        }
    }
}
