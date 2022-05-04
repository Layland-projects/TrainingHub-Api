using Microsoft.Extensions.Logging;
using Prometheus;
using TrainingHub.Infrastructure.Abstractions;

namespace TrainingHub.Infrastructure.Implementations.Prometheus
{
    public class PrometheusMetricReporter : IMetricReporter
    {
        private readonly ILogger<PrometheusMetricReporter> _logger;
        private readonly Counter _requestCounter;
        private readonly Histogram _responseTimeHistogram;
        public PrometheusMetricReporter(
            ILogger<PrometheusMetricReporter> logger
            )
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _requestCounter = Metrics.CreateCounter("total_requests", 
                "The total number of requests serviced by this API.", 
                new CounterConfiguration
                {
                    LabelNames = new [] { "controller", "path", "status_code", "method" }
                });
            _responseTimeHistogram = Metrics.CreateHistogram("request_duration_seconds",
                "The processing duration of a request in seconds.",
                new HistogramConfiguration
                {
                    Buckets = Histogram.ExponentialBuckets(0.01, 2, 10),
                    LabelNames = new [] { "status_code", "method" }
                });
        }
        public void RegisterRequest()
        {
            _requestCounter.Inc();
        }

        public void RegisterRequest(string? controllerName = null, string? path = null, int? statusCode = null, string? method = null)
        {
            _requestCounter.WithLabels(controllerName, path, statusCode?.ToString(), method).Inc();
            RegisterRequest();
        }

        public void RegisterResponseTime(int statusCode, string method, TimeSpan elapsed)
        {
            _responseTimeHistogram.WithLabels(statusCode.ToString(), method).Observe(elapsed.TotalSeconds);
        }
    }
}
