using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingHub.Models.Global;
using TrainingHub.Models.Models;
using static TrainingHub.Models.Enums.Enums;

namespace TrainingHub.Infrastructure.Abstractions
{
    public interface IActivityService
    {
        Task<Result<IEnumerable<Activity>>> GetPaged(int pageNo = 0, int pageSize = 10);
        Task<Result<IEnumerable<Activity>>> GetPagedByType(ActivityType type, int pageNo = 0, int pageSize = 10);
        Task<Result<IEnumerable<Activity>>> SearchPaged(string searchTerm, int pageNo = 0, int pageSize = 10);
        Task<Result<Activity>> GetById(int id);
        Task<Result<bool>> ActivityExists(int id);
        Task<Result<bool>> ActivityExists(string title);
        Task<Result> Add(string title, string description, ActivityType type, bool isBodyWeight, string image);
        Task<Result> Disable(int id);
        Task<Result> Disable(string title);
        Task<Result> Enable(int id);
        Task<Result> Enable(string title);
        Task<Result> Update(int id, string title, string description, ActivityType type, bool isBodyWeight, string image);
    }
}
