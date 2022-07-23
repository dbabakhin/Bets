using Bets.Domain.Interfaces;
using Bets.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bets.Infrastructure.DI
{

    public static class ServiceCollectionExtension
    {
        const string BETS_CONNECTION = "BetsConnection";

        public static IServiceCollection AddBetsDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BetsConnectionConfig>(configuration.GetSection(BETS_CONNECTION));
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IBetsRepository, BetsRepository>();

            return services;
        }
    }
}
