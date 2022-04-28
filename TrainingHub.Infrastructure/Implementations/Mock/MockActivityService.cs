using TrainingHub.Infrastructure.Abstractions;
using TrainingHub.Models.Enums;
using TrainingHub.Models.Global;
using TrainingHub.Models.Models;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.Infrastructure.Implementations.Mock
{
    public class MockActivityService : IActivityService
    {
        public static readonly List<Activity> mockActivities = new List<Activity>
        {
            new Activity
            {
                Id = 1,
                Title = "Push ups",
                Description = "Body weight upper body exercise",
                Type = ActivityType.Repetitions,
                Image = "placeholder.png",
                IsBodyWeight = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = "System",
            },
            new Activity
            {
                Id = 2,
                Title = "Bicep curls",
                Description = "Bicep accessory exercies",
                Type = ActivityType.Repetitions,
                Image = "placeholder.png",
                IsBodyWeight = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = "System"
            },
            new Activity
            {
                Id = 3,
                Title = "Situps",
                Description = "Abdominal accessory exercise",
                Type = ActivityType.Repetitions,
                Image = "placeholder.png",
                IsBodyWeight = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = "System"
            },
        };
        private static int currentId = 3;
        private readonly ITimestampService timestampService;
        private readonly IUserService userService;

        public MockActivityService(ITimestampService timestampService,
            IUserService userService)
        {
            this.timestampService = timestampService;
            this.userService = userService;
        }
        public Task<Result<bool>> ActivityExists(int id)
        {
            if (mockActivities.SingleOrDefault(x => x.Id == id) != null)
            {
                return Task.FromResult(Result<bool>.SuccessFrom(true));
            }
            return Task.FromResult(Result<bool>.SuccessFrom(false));
        }

        public Task<Result<bool>> ActivityExists(string title)
        {
            if (mockActivities.SingleOrDefault(x => x.Title.ToLower().Trim() == title.ToLower().Trim()) != null)
            {
                return Task.FromResult(Result<bool>.SuccessFrom(true));
            }
            return Task.FromResult(Result<bool>.SuccessFrom(false));
        }

        public async Task<Result> Add(string title, string description, Enums.ActivityType type, bool isBodyWeight, string image)
        {
            var exists = await this.ActivityExists(title);
            if (exists.Data)
            {
                return Result.Failure("Activity with that title already exists");
            }
            var username = GetCurrentUsername();

            mockActivities.Add(new Activity
            {
                Id = ++currentId,
                Title = title.Trim(),
                Description = description.Trim(),
                Image = image.Trim(),
                Type = type,
                IsBodyWeight = isBodyWeight,
                CreatedAt = this.timestampService.GetDateTimeOffset(),
                UpdatedAt = this.timestampService.GetDateTimeOffset(),
                CreatedBy = username,
                UpdatedBy = username
            });
            return Result.Success();
        }

        public Task<Result<Activity>> GetById(int id)
        {
            var a = mockActivities.SingleOrDefault(x => x.Id == id);
            if (a == null)
            {
                return Task.FromResult(Result<Activity>.FailureFrom(null, $"Id: {id} not found"));
            }
            return Task.FromResult(Result<Activity>.SuccessFrom(a));
        }

        public Task<Result<IEnumerable<Activity>>> SearchPaged(string searchTerm, int pageNo = 0, int pageSize = 10)
        {
            var a = mockActivities.Where(x => x.Title.ToLower().Trim().Contains(searchTerm.ToLower().Trim()) ||
            x.Description.ToLower().Trim().Contains(searchTerm.ToLower().Trim()))
                .DistinctBy(x => x.Id)
                .OrderBy(x => x.Id)
                .Skip(pageNo * pageSize)
                .Take(pageSize);
            if (a == null || !a.Any())
            {
                return Task.FromResult(Result<IEnumerable<Activity>>.FailureFrom(Enumerable.Empty<Activity>(), $"Term: {searchTerm} not found"));
            }
            return Task.FromResult(Result<IEnumerable<Activity>>.SuccessFrom(a));
        }

        public Task<Result<IEnumerable<Activity>>> GetPaged(int pageNo = 0, int pageSize = 10)
        {
            var a = mockActivities.Skip(pageNo * pageSize).Take(pageSize);
            return Task.FromResult(Result<IEnumerable<Activity>>.SuccessFrom(a));
        }

        public Task<Result<IEnumerable<Activity>>> GetPagedByType(ActivityType type, int pageNo = 0, int pageSize = 10)
        {
            var a = mockActivities.Where(x => x.Type == type).Skip(pageNo * pageSize).Take(pageSize);
            return Task.FromResult(Result<IEnumerable<Activity>>.SuccessFrom(a));
        }

        public Task<Result> Disable(int id)
        {
            var a = mockActivities.SingleOrDefault(x => x.Id == id);
            if (a == null)
            {
                return Task.FromResult(Result.Failure($"Id: {id} not found"));
            }
            var username = GetCurrentUsername();
            a.DeletedAt = timestampService.GetDateTimeOffset();
            a.DeletedBy = username;
            return Task.FromResult(Result.Success());
        }

        public Task<Result> Disable(string title)
        {
            var a = mockActivities.SingleOrDefault(x => x.Title == title);
            if (a == null)
            {
                return Task.FromResult(Result.Failure($"Title: {title} not found"));
            }
            var username = GetCurrentUsername();
            a.DeletedAt = timestampService.GetDateTimeOffset();
            a.DeletedBy = username;
            return Task.FromResult(Result.Success());
        }

        public Task<Result> Enable(int id)
        {
            var a = mockActivities.SingleOrDefault(x => x.Id == id);
            if (a == null)
            {
                return Task.FromResult(Result.Failure($"Id: {id} not found"));
            }
            a.DeletedAt = null;
            a.DeletedBy = null;
            return Task.FromResult(Result.Success());
        }

        public Task<Result> Enable(string title)
        {
            var a = mockActivities.SingleOrDefault(x => x.Title.ToLower().Trim() == title.ToLower().Trim());
            if (a == null)
            {
                return Task.FromResult(Result.Failure($"Title: {title} not found"));
            }
            a.DeletedAt = null;
            a.DeletedBy = null;
            return Task.FromResult(Result.Success());
        }

        public Task<Result> Update(int id, string title, string description, ActivityType type, bool isBodyWeight, string image)
        {
            var a = mockActivities.SingleOrDefault(x => x.Id == id);
            if (a == null)
            {
                return Task.FromResult(Result.Failure($"Id: {id} not found"));
            }
            a.Title = !string.IsNullOrEmpty(title) ? title.Trim() : a.Title;
            a.Description = !string.IsNullOrEmpty(description) ? description.Trim() : a.Description;
            a.Type = type;
            a.IsBodyWeight = isBodyWeight;
            a.Image = !string.IsNullOrEmpty(image) ? image.Trim() : a.Image;
            return Task.FromResult(Result.Success());
        }

        private string GetCurrentUsername() => IUserService.SignedInUser != null ?
                IUserService.SignedInUser.Username ?? $"{IUserService.SignedInUser?.Title} {IUserService.SignedInUser?.FirstName} {IUserService.SignedInUser?.LastName}" :
                "System";
    }
}
