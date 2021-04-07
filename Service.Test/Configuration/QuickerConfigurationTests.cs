using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quicker.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Quicker.Service.Test.Configuration
{
    public class QuickerConfigurationTests
    {
        [Fact]
        public void AddQuickerConfiguration_DefaultConfig_Success()
        {
            var provider = new ServiceCollection()
                .AddQuickerConfiguration()
                .BuildServiceProvider();

            var quickerConfiguration = provider.GetRequiredService<IOptions<QuickerConfiguration>>().Value;

            Assert.True(quickerConfiguration.UseAutoMapper);
            Assert.True(quickerConfiguration.UseLogger);
        }

        [Fact]
        public void AddQuickerConfiguration_CustomConfig_Success()
        {
            var provider = new ServiceCollection()
                .AddQuickerConfiguration(e => {
                    e.UseLogger = false;
                    e.UseAutoMapper = false;
                })
                .BuildServiceProvider();

            var quickerConfiguration = provider.GetRequiredService<IOptions<QuickerConfiguration>>().Value;

            Assert.False(quickerConfiguration.UseAutoMapper);
            Assert.False(quickerConfiguration.UseLogger);
        }
    }
}
