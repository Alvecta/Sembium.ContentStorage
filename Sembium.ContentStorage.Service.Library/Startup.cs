using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace Sembium.ContentStorage.Service.Library
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    options.ReturnHttpNotAcceptable = false;   // remove this in the furute
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            ServicesConfig.ConfigureServices(services, Configuration);

            return AutofacRegistrations.ConfigureServices(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(
                typeof(ErrorHandlingMiddleware),
                loggerFactory,
                typeof(Sembium.ContentStorage.Common.UserException),
                typeof(Sembium.ContentStorage.Common.UserAuthenticationException),
                typeof(Sembium.ContentStorage.Service.Security.UserAuthorizationException)
            );

            app.UseMvc();

            ServicesConfig.Configure(loggerFactory, Configuration);
        }
    }
}
