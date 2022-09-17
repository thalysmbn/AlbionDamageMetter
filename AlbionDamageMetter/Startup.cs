using AlbionDamageMetter.Services;
using Serilog;

namespace AlbionDamageMetter
{
    public class Startup
    {
        public Startup(IWebHostEnvironment currentEnvironment, IConfiguration configuration)
        {
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHostedService<CaptureDeviceNetwork>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();
        }
    }
}
