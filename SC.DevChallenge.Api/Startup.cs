using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SC.DevChallenge.Api.Config.Filters;
using SC.DevChallenge.Api.Config.Middlewares;
using SC.DevChallenge.Api.Database;
using SC.DevChallenge.Api.Services;
using SC.DevChallenge.Api.Services.Abstractions;

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
            services.AddControllers(options => options.Filters.Add<ModelStateFilter>())
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<IAssemblyMarker>());
            
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SC.DevChallenge.Api", Version = "v1" })
            );

            // TODO: Move connection string to appsettings
            services.AddDbContext<ApplicationDbContext>();

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            
            services.AddTransient<IPriceService, PriceService>();
            


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
