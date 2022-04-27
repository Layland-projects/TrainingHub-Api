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
                AuthId = 1,
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
                AuthId = 2,
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
                Trainees = new List<User>(),
                Trainer = null,
                TrainerId = null,
            },
            new User
            {
                Id = 3,
                AuthId = 3,
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

        static MockUserService()
        {
            var u = mockUsers.Where(x => x.Id == 2 || x.Id == 3);
            var session = new Session
            {
                Trainee = u.First(x => x.Id == 3),
                TraineeId = 3,
                Trainer = u.First(x => x.Id == 2),
                TrainerId = 2,
                IsCompleted = true,
                ScheduledFor = DateTime.Now.AddDays(-1),
                Sets = new List<ActivitySet>()
                {
                    new ActivitySet
                    {
                        Activity = new Activity
                        {
                            Id = 1,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            CreatedBy = "System",
                            UpdatedBy = "System",
                            Title = "Push ups",
                            Description = "Upper body bw exercise",
                            Image = "placeholder.png",
                            IsBodyWeight = true,
                            Type = ActivityType.Repetitions
                        },
                        ActivityId = 1,
                        ActivityCount = 5,
                        Repetitions = 10
                    }
                }
            };
            foreach(var usr in u)
            {
                usr.Sessions.Add(session);
            }
        }
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
                return Task.FromResult(Result<User>.FailureFrom(null)) ;
            }
            var rand = new Random();
            var user = new User
            {
                Id = ++currentId,
                AuthId = rand.Next(1, 100),
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
            return Task.FromResult(Result<User>.SuccessFrom(user));
        }

        public Task<Result<User>> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            var user = mockUsers.Where(x => x.Id == id).SingleOrDefault();
            if (user == null)
            {
                return Task.FromResult(Result<User>.FailureFrom(null, $"User /w id: {id} not found"));
            }
            return Task.FromResult(Result<User>.SuccessFrom(user));
        }

        public Task<Result<IEnumerable<User>>> GetUsersAsync(int pageSize, int pageNum, CancellationToken cancellationToken)
        {
            var users = mockUsers.Skip(pageNum * pageSize).Take(pageSize);
            if (users == null)
            {
                return Task.FromResult(Result<IEnumerable<User>>.FailureFrom(Enumerable.Empty<User>(),
                    $"No entries for pageNum: {pageNum}, pageSize: {pageSize}"));
            }
            return Task.FromResult(Result<IEnumerable<User>>.SuccessFrom(users));
        }

        public Task<Result> UpdateEmailAsync(int id, string email)
        {
            var user = mockUsers.Where(x => x.Id == id).SingleOrDefault();
            if (user != null)
            {
                user.Email = email;
                return Task.FromResult(Result.Success());
            }
            return Task.FromResult(Result.Failure($"User for id: {id} not found"));
        }

        public Task<Result> UpdatePasswordAsync(int id, string password)
        {
            var user = mockUsers.Where(x => x.Id == id).SingleOrDefault();
            if (user != null)
            {
                //not storing password here is it's a mock.
                return Task.FromResult(Result.Success());
            }
            return Task.FromResult(Result.Failure($"User for id: {id} not found"));
        }

        public Task<Result> UpdateUsernameAsync(int id, string username) 
        {
            var user = mockUsers.Where(x => x.Id == id).SingleOrDefault();
            if (user != null)
            {
                user.Username = username;
            }
            return Task.FromResult(Result.Failure($"User for id: {id} not found"));
        }

        public Task<Result<int>> SignIn(string username, string password)
        {
            var user = mockUsers.SingleOrDefault(x => x.Username == username || x.Email == username);
            if (user == null)
            {
                return Task.FromResult(Result<int>.FailureFrom(0, $"No user exists for provided username & password"));
            }
            SignedInUser = user;
            return Task.FromResult(Result<int>.SuccessFrom(user.Id));
        }

        public Task<Result> SignOut(int id)
        {
            if (SignedInUser == null || SignedInUser.Id != id)
            {
                return Task.FromResult(Result.Failure($"User with id: {id} is not currently signed in"));
            }
            SignedInUser = null;
            return Task.FromResult(Result.Success());
        }

        public Task<Result> ForgotPassword(string email)
        {
            if (mockUsers.Any(x => x.Email == email))
            {
                // pretend to send e-mail
                return Task.FromResult(Result.Success());
            }
            return Task.FromResult(Result.Failure($"email: {email} doesn't exist"));
        }

        public Task<Result> UpdateUserAsync(int id, string title, string firstName, string lastName)
        {
            var user = mockUsers.SingleOrDefault(x => x.Id == id);
            if (user == null)
            {
                return Task.FromResult(Result.Failure($"No user found for id: {id}"));
            }
            user.Title = title != user.Title ? title : user.Title;
            user.FirstName = firstName != user.FirstName ? firstName : user.FirstName;
            user.LastName = lastName != user.LastName ? lastName : user.LastName;
            return Task.FromResult(Result.Success());
        }
    }
}
