using Microsoft.Extensions.DependencyInjection;
using TrainingHub.Infrastructure.Abstractions;

namespace TrainingHub.Infrastructure.Implementations.Mock
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMockInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IUserService, MockUserService>();
            services.AddSingleton<ITimestampService, MockTimestampService>();
            services.AddSingleton<IActivityService, MockActivityService>();
            services.AddSingleton<ISessionService, MockSessionService>();
            return services;
        }
    }
}
