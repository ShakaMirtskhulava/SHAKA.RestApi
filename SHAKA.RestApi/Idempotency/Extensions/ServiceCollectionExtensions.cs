using Microsoft.Extensions.DependencyInjection;
using System;

namespace SHAKA.RestApi.Idempotency.Extensions
{
    /// <summary>
    /// Extension methods for configuring idempotency services in ASP.NET Core applications.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the core idempotency services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureOptions">An optional action to configure idempotency options.</param>
        /// <returns>The service collection for method chaining.</returns>
        /// <remarks>
        /// This method adds the core services required by the idempotency system.
        /// You must also register an implementation of <see cref="IIdempotencyStore"/> separately.
        /// </remarks>
        public static IServiceCollection AddIdempotency(
            this IServiceCollection services,
            Action<IdempotencyOptions>? configureOptions = null)
        {
            // Register options
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }
            
            return services;
        }
    }
}
