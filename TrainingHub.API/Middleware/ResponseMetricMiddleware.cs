using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using TrainingHub.API.Controllers;
using TrainingHub.Infrastructure.Abstractions;

namespace TrainingHub.API.Middleware
{

    public class ResponseMetricMiddleware
    {
        private readonly RequestDelegate _request;

        public ResponseMetricMiddleware(RequestDelegate request)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public async Task Invoke(HttpContext httpContext, IMetricReporter reporter)
        {
            var path = httpContext.Request.Path.Value;
            var controllerNames = Assembly.GetAssembly(typeof(ControllerMarker))?.GetTypes().Where(x => x.IsSubclassOf(typeof(ControllerBase))).Select(x => x.Name.Replace("Controller", ""));
            if (path == "/metrics")
            {
                await _request.Invoke(httpContext);
                return;
            }
            var sw = Stopwatch.StartNew();

            try
            {
                await _request.Invoke(httpContext);
            }
            finally
            {
                sw.Stop();
                if (controllerNames != null && controllerNames.Any())
                {
                    var name = controllerNames.FirstOrDefault(x => path.Contains(x.ToLower()));
                    if (string.IsNullOrEmpty(name))
                    {
                        reporter.RegisterRequest(null , path, httpContext.Response.StatusCode, httpContext.Request.Method);
                    }
                    else
                    {
                        reporter.RegisterRequest(name, path, httpContext.Response.StatusCode, httpContext.Request.Method);
                    }
                }
                reporter.RegisterRequest();
                reporter.RegisterResponseTime(httpContext.Response.StatusCode, httpContext.Request.Method, sw.Elapsed);
            }
        }
    }
}
