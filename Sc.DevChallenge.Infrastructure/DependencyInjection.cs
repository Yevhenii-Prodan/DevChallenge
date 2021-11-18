using Microsoft.Extensions.DependencyInjection;
using Sc.DevChallenge.Application.Common.Interfaces;
using Sc.DevChallenge.Infrastructure.Database;

namespace Sc.DevChallenge.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // TODO: Move connection string to appsettings
            services.AddDbContext<ApplicationDbContext>();

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            return services;
        }
    }
}