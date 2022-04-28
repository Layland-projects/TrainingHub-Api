namespace TrainingHub.Infrastructure.Abstractions
{
    public interface IMetricReporter
    {
        void RegisterRequest();
        void RegisterRequest(string? controllerName = null, string? path = null, int? statusCode = null, string? method = null);
        void RegisterResponseTime(int statusCode, string method, TimeSpan elapsed);
    }
}
