using System.Reflection;

using CorrelationId;
using CorrelationId.DependencyInjection;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Prometheus;

using RefactorThis.DataAccess;
using RefactorThis.DataAccess.EntityService;
using RefactorThis.Mvc;
using RefactorThis.Mvc.Extensions;
using RefactorThis.Mvc.Filters;
using RefactorThis.Mvc.Middlewares;
using RefactorThis.Providers;
using RefactorThis.RequestsPipeline;

using Serilog;

namespace RefactorThis;

public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCorrelationId();
        app.UseSerilogRequestLogging();
        app.UseMetricServer();
        app.UseHttpMetrics();
        app.UseCors("AllowAll");
        app.UseRouting();

        app.UseMiddleware<ApiTokenAuthMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(
            c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Refactor This API Documentation"); });

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseEndpoints(
            endpoints => { endpoints.MapControllers(); });
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddValidatorsFromAssemblies(new[] { typeof(Startup).Assembly });
        services.AddMediatR(typeof(Startup).Assembly);

        services.AddApiVersioning(
            options =>
                {
                    options.ReportApiVersions = false;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new(1, 0);
                });

        services.AddDefaultCorrelationId(
            x =>
                {
                    x.RequestHeader = "X-CorrelationId";
                    x.AddToLoggingScope = true;
                    x.IncludeInResponse = false;
                });

        services.AddCors(
            options =>
                {
                    options.AddPolicy(
                        "AllowAll",
                        builder =>
                            {
                                builder.AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .SetIsOriginAllowed(_ => true)
                                    .AllowCredentials();
                            });
                });

        services.AddControllers(
                options =>
                    {
                        options.UseGlobalRoutePrefix("api/v{version:apiVersion}");
                        options.Filters.Add<HttpResponseExceptionFilter>();
                    })
            .ConfigureApiBehaviorOptions(
                options =>
                    {
                        options.InvalidModelStateResponseFactory = context =>
                            {
                                var errors = context.ModelState
                                    .Where(x => x.Value!.Errors.Any())
                                    .ToDictionary(
                                        kvp => kvp.Key,
                                        kvp => (object?)kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                                return HttpResponseFactory.ModelValidationErrorsResponse(errors);
                            };
                    })
            .AddNewtonsoftJson();

        services.AddSwaggerGen(
            c =>
                {
                    c.CustomSchemaIds(x => x.FullName);

                    c.AddSecurityDefinition(
                        "ApiKey",
                        new()
                        {
                            In = ParameterLocation.Header,
                            Description = "Please insert your API token",
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey,
                        });

                    c.AddSecurityRequirement(
                        new()
                        {
                            {
                                new()
                                {
                                    Reference = new() { Type = ReferenceType.SecurityScheme, Id = "ApiKey" },
                                },
                                Array.Empty<string>()
                            },
                        });

                    c.SwaggerDoc(
                        "v1",
                        new()
                        {
                            Title = "ProductsApi API",
                            Version = "v1",
                        });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);

                    c.TagActionsBy(
                        api =>
                            {
                                if (api.GroupName != null)
                                {
                                    return new[] { api.GroupName };
                                }

                                if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                                {
                                    return new[] { controllerActionDescriptor.ControllerName };
                                }

                                throw new InvalidOperationException("Unable to determine tag for endpoint.");
                            });

                    c.DocInclusionPredicate((_, _) => true);
                });

        services
            .AddScoped<IDateTimeProvider, DateTimeProvider>()
            .AddScoped<IEntityService, EntityService>()
            .AddScoped<IReadOnlyEntityService, EntityService>()
            .AddScoped<HttpResponseExceptionFilter>();

        services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionScopeBehavior<,>));

        services.AddDbContext<Context>(
            options =>
                {
                    options.UseSqlite(this.configuration.GetConnectionString("ProductsApi"));
                });
    }
}