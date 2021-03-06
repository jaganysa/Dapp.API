﻿using Dapp.Api.Data.Infrastructure;
using Dapp.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dapp.Api.Configuration
{
    /// <summary>
    /// IOC contaner start-up configuration
    /// </summary>
    public static class IocContainerConfiguration
    {
        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void ConfigureService(IServiceCollection services, IConfigurationRoot configuration)
        {
            // Services
            services.AddTransient<IDeviceService, DeviceService>();

            // Infrastructure
            services.AddTransient<IConnectionFactory, ConnectionFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
