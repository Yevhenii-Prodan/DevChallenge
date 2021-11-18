using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SC.DevChallenge.Api.Utils.Filters;
using SC.DevChallenge.Api.Utils.Middlewares;
using Sc.DevChallenge.Application.Services;
using Sc.DevChallenge.Application.Services.Abstractions;
using Sc.DevChallenge.Infrastructure;

namespace SC.DevChallenge.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options => options.Filters.Add<ModelStateFilter>())
                .AddNewtonsoftJson()
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<IAssemblyMarker>());
            
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SC.DevChallenge.Api", Version = "v1" })
            );

            services.AddInfrastructure();

            
            services.AddTransient<IPriceService, PriceService>();
            services.AddTransient<IPriceCalculator, PriceCalculator>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SC.DevChallenge.Api v1"));
            }

            app.UseCustomExceptionHandler();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
