using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Random.Web.Core
{
    [AppStartup(800)]
    public sealed class RandomWebCoreStartup : AppStartup
    {
        /*public void ConfigureServices(IServiceCollection services)
        {
            services.AddSpecificationDocuments();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSpecificationDocuments();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }*/
    }
}
