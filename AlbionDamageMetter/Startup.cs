using AlbionDamageMetter.Albion;
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
            services.AddSingleton<AlbionClusterData>()
                .AddSingleton<AlbionEntityData>();
            services.AddHostedService<CaptureDeviceNetwork>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });

        }
    }
}
