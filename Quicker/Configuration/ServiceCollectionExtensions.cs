using Quicker.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class QuickerConfigurationExtensions
    {
        public static void AddQuickerConfiguration(this IServiceCollection services, Action<QuickerConfiguration> customConfig) 
        {
            services.AddOptions<QuickerConfiguration>();

            if (customConfig != null)
                services.Configure(customConfig);
        }
        
        public static void AddQuickerConfiguration(this IServiceCollection services) 
            => AddQuickerConfiguration(null);
    }
}
