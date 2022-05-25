using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingHub.Infrastructure.Abstractions;
using TrainingHub.Models.Global;
using TrainingHub.Models.Models;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.Infrastructure.Implementations.Mock
{
    public class MockSessionService : ISessionService
    {
        //dummy values
        private const int adminId = 1;
        private const int trainerId = 2;
        private const int traineeId = 3;
        private const int pushupsId = 1;
        private const int bicepCurlsId = 2;
        private const int situpsId = 3;

        //actual imports
        private readonly ITimestampService timestampService;
        private readonly IActivityService activityService;
        private readonly IUserService userService;

        public MockSessionService(ITimestampService timestampService,
            IActivityService activityService,
            IUserService userService)
        {
            this.timestampService = timestampService;
            this.activityService = activityService;
            this.userService = userService;
        }


        private static readonly List<Session> mockSessions = new()
        {
            new()
            {
                Id = 1,
                TraineeId = traineeId,
                TrainerId = adminId,
                IsCompleted = true,
                ScheduledFor = DateTimeOffset.Now.AddDays(-1),
                Sets = new List<ActivitySet>
                {
                    new ()
                    {
                        Id = 1,
                        ActivityId = pushupsId,
                        Repetitions = 15,
                        ActivityCount = 5,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = "System",
                        UpdatedBy = "System",
                    },
                    new ()
                    {
                        Id = 2,
                        ActivityId = bicepCurlsId,
                        Repetitions = 10,
                        WeightKgs = 15,
                        ActivityCount = 5,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = "System",
                        UpdatedBy = "System",
                    },
                    new ()
                    {
                        Id = 3,
                        ActivityId = situpsId,
                        Repetitions = 15,
                        ActivityCount = 5,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = "System",
                        UpdatedBy = "System"
                    }
                },
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = "System",
                UpdatedBy = "System"
            }
        };

        public Task<Result<Session>> GetSession(int id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<Result<Session>>(cancellationToken);
            }
            var session = mockSessions.SingleOrDefault(x => x.Id == id);
            if (session == null)
            {
                return Task.FromResult(Result.FailureFrom(null as Session, $"No session exists with id: {id}"));
            }
            return Task.FromResult(Result<Session>.SuccessFrom(session));
        }

        public async Task<Result<ICollection<Session>>> GetSessionsByAzureUserId(Guid azureId, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Result.FailureFrom<ICollection<Session>>(new List<Session>(), "Cancellation token received");
            }
            var user = await this.userService.GetUserByAzureIdAsync(azureId, cancellationToken);
            if (user == null || user.Status != ResultStatus.Success)
            {
                return Result.FailureFrom<ICollection<Session>>(new List<Session>(), $"No user exists with azureId: {azureId}");
            }
            var sessions = mockSessions.Where(x => x.TrainerId == user.Data.Id).Union(mockSessions.Where(x => x.TraineeId == user.Data.Id));
            if (sessions == null || !sessions.Any())
            {
                return Result.FailureFrom<ICollection<Session>>(new List<Session>(), $"No sessions found for user with azureId: {azureId}");
            }
            return Result.SuccessFrom<ICollection<Session>>(sessions.ToList());
        }

        public Task<Result<ICollection<Session>>> GetSessionsByUserId(int userId, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<Result<ICollection<Session>>>(cancellationToken);
            }
            var sessions = mockSessions.Where(x => x.TraineeId == userId).Union(mockSessions.Where(x => x.TrainerId == userId));
            if (sessions == null || !sessions.Any())
            {
                return Task.FromResult(Result.FailureFrom<ICollection<Session>>(new List<Session>(), $"No sessions for user with id: {userId}"));
            }
            return Task.FromResult(Result.SuccessFrom<ICollection<Session>>(sessions.ToList()));
        }

        public Task<Result<ICollection<Session>>> GetSessionsPaged(CancellationToken cancellationToken, int pageSize = 10, int pageNo = 0)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<Result<ICollection<Session>>>(cancellationToken);
            }
            var sessions = mockSessions.Skip(pageNo * pageSize).Take(pageSize);
            if (sessions == null || !sessions.Any())
            {
                return Task.FromResult(Result.SuccessFrom<ICollection<Session>>(new List<Session>()));
            }
            return Task.FromResult(Result.SuccessFrom<ICollection<Session>>(sessions.ToList()));
        }
    }
}
