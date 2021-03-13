using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quicker.Services.Test.Fake;
using Test.Common;
using Test.Common.Mapper;
using Test.Common.Repository;

namespace Quicker.Integration.Test
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddQuickerConfiguration(config =>
            {
                config.UseLogger = true;
                config.UseAutoMapper = true;
            });

            services.AddScoped<DbContext, TestContext>(e =>
                new ConnectionFactory()
                .CreateContextForSQLite()
            );
            services.AddScoped(e => 
                new MapperConfiguration(config =>
                {
                    config.AddProfile<TestModelRelationMapper>();
                })
                .CreateMapper()
            );

            services.AddScoped<FakeCloseService>();
            services.AddScoped<FakeCloseServiceDTO>();
            services.AddScoped<FakeOpenService>();
            services.AddScoped<FakeOpenServiceDTO>();
            services.AddScoped<FakeFullService>();
            services.AddScoped<FakeFullServiceDTO>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            }); 
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
