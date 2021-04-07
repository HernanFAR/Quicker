using Quicker.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    #warning Agregar summaries de estas funciones.
    public static class QuickerConfigurationExtensions
    {
        public static IServiceCollection AddQuickerConfiguration(this IServiceCollection services, Action<QuickerConfiguration> customConfig) 
        {
            services.AddOptions<QuickerConfiguration>();

            if (customConfig != null)
                services.Configure(customConfig);

            return services;
        }
        
        public static IServiceCollection AddQuickerConfiguration(this IServiceCollection services) 
            => services.AddQuickerConfiguration(null);
    }
}
