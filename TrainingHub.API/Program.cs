using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Microsoft.OpenApi.Models;
using Prometheus;
using System.Reflection;
using TrainingHub.API.Controllers;
using TrainingHub.API.Middleware;
using TrainingHub.Infrastructure.Implementations.Mock;
using TrainingHub.Infrastructure.Implementations.Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddMockInfrastructure();
builder.Services.AddPrometheus();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200", "https://localhost:4200");
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowAnyHeader();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var scopes = Assembly.GetAssembly(typeof(ControllerMarker))?.DefinedTypes.SelectMany(t => t.GetMethods())
    .SelectMany(m => m?.GetCustomAttribute<RequiredScopeAttribute>()?.Arguments ?? Array.Empty<object>())
    .SelectMany(x => x as string[] ?? Array.Empty<string>())
    .Where(x => x != null && x != default && x.Length > 0).Distinct();

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{builder.Configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize"),
                TokenUrl = new Uri($"https://login.microsoftonline.com/{builder.Configuration["AzureAd:TenantId"]}/oauth2/v2.0/token"),
                Scopes = scopes?
                .Select(x => builder.Configuration["AzureAd:ScopeBase"] + x)
                .ToDictionary(x => x)
            }
        },
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                },
                Scheme = "oauth2",
                Name = "oauth2",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrainingHub-API v1");
        c.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
        c.OAuthClientSecret(builder.Configuration["AzureAd:Secret"]);
        c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    });
}

app.UseMetricServer();
app.UseMiddleware<ResponseMetricMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.MapControllers();

app.Run();
