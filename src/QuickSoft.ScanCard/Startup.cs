using System;
using System.Collections.Generic;
using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using QuickSoft.ScanCard.Infrastructure;
using QuickSoft.ScanCard.Infrastructure.Errors;
using QuickSoft.ScanCard.Infrastructure.Security;

namespace QuickSoft.ScanCard
{
    public class Startup
    {
        private const string DEFAULT_DATABASE_CONNECTION_STRING = "Filename=quicksoft.db";
        private const string DEFAULT_DATABASE_PROVIDER = "sqlite";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DbContextTransactionPipeLineBehavior<,>));

            // take the connection string from the environment variable or use hard-coded database name
            var connectionString = Configuration.GetValue<string>("ASPNETCORE_QuickSoft_ConnectionString") ?? DEFAULT_DATABASE_CONNECTION_STRING;
            // // take the database provider from the environment variable or use hard-coded database provider
            var databaseProvider = Configuration.GetValue<string>("ASPNETCORE_QuickSoft_DatabaseProvider");
            if (string.IsNullOrWhiteSpace(databaseProvider))
            {
                databaseProvider = DEFAULT_DATABASE_PROVIDER;
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                switch (databaseProvider.ToLower().Trim())
                {
                    case "sqlite":
                        options.UseSqlite(connectionString);
                        break;
                    case "sqlserver":
                        // only works in windows container
                        options.UseSqlServer(connectionString);
                        break;
                    default:
                        throw new Exception("Database provider unknown. Please check configuration");
                }
            });

            services.AddLocalization(x => x.ResourcesPath = "Resources");

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {   new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()}
                });
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "QuickSoft API", Version = "v1" });
                x.CustomSchemaIds(y => y.FullName);
                x.DocInclusionPredicate((version, apiDescription) => true);
                x.TagActionsBy(y => new List<string>()
                {
                    y.GroupName
                });
            });

            services.AddCors();
            services.AddMvc(opt =>
                {
                    opt.Conventions.Add(new GroupByApiRootConvention());
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                    opt.EnableEndpointRouting = false;
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                })
                .AddFluentValidation(cfg =>
                {
                    cfg.RegisterValidatorsFromAssemblyContaining<Startup>();
                });

            services.AddAutoMapper(GetType().Assembly);

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            // services.AddScoped<IProfileReader, ProfileReader>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // services.AddJwt();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilogLogging();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseAuthentication();
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui assets(HTML, JS, CSS etc.)
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "QuickSoft API V1");
            });

            app.ApplicationServices.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();
        }
    }
}