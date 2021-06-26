using Calzolari.Grpc.AspNetCore.Validation;
using GrpcCountryApi.Repository;
using GrpcCountryApi.Repository.Database;
using GrpcCountryApi.Repository.Interfaces;
using GrpcCountryApi.Services;
using GrpcCountryApi.Services.Interfaces;
using GrpcCountryApi.Web.Logging;
using GrpcCountryApi.Web.Services;
using GrpcCountryApi.Web.Validator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Reflection;
using CountryGrpcServiceV1 = GrpcCountryApi.Web.Services.CountryGrpcService;

namespace GrpcCountryApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CountryDbContext>(options => options.UseInMemoryDatabase(databaseName: "country_db"));

            var serviceProvider = services.BuildServiceProvider();
            using (var context = new CountryDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<CountryDbContext>>()))
            {
                context.Country.AddRange(
                    new GrpcCountryApi.Domain.Entities.Country
                    {
                        CountryId = 1,
                        CountryName = "Canada",
                        Description = "Maple leaf country"
                    },
                    new GrpcCountryApi.Domain.Entities.Country
                    {
                        CountryId = 2,
                        CountryName = "Japon",
                        Description = "Rising sun country"
                    },
                    new GrpcCountryApi.Domain.Entities.Country
                    {
                        CountryId = 3,
                        CountryName = "Australia",
                        Description = "Wallabies country"
                    });

                context.SaveChanges();
            }

            services.AddGrpc(options =>
            {
                options.EnableMessageValidation();
                options.Interceptors.Add<LoggerInterceptor>();
            });

            services.AddCors(o =>
            {
                o.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.WithExposedHeaders("Grpc-Status", "Grpc-Message");
                });
            });

            services.AddValidator<CountryCreateRequestValidator>();

            services.AddGrpcValidation();

            services.AddAutoMapper(Assembly.Load("DemoGrpc.Web"));

            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICountryRepository, EFCountryRepository>();

            //services.AddApplicationInsightsTelemetry();

            services.AddSingleton<ProtoService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
                var protoService = endpoints.ServiceProvider.GetRequiredService<ProtoService>();

                endpoints.MapGrpcService<CountryGrpcServiceV1>().RequireCors("MyPolicy").EnableGrpcWeb();

                endpoints.MapGet("/protos", async context =>
                {
                    await context.Response.WriteAsync(await protoService.Get());
                });

                endpoints.MapGet("/protos/v{version:int}/{protoName}", async context =>
                {
                    var version = int.Parse((string)context.Request.RouteValues["version"]);
                    var protoName = (string)context.Request.RouteValues["protoName"];

                    var filePath = protoService.Get(version, protoName);

                    if (filePath != null)
                    {
                        await context.Response.SendFileAsync(filePath);
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    }
                });
            });
        }
    }
}
