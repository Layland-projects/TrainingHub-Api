using Microsoft.Extensions.DependencyInjection;
using TrainingHub.Infrastructure.Abstractions;

namespace TrainingHub.Infrastructure.Implementations.Prometheus
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPrometheus(this IServiceCollection services)
        {
            services.AddSingleton<IMetricReporter, PrometheusMetricReporter>();
            return services;
        }
    }
}
