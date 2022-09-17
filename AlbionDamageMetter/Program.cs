using AlbionDamageMetter;
using AlbionDamageMetter.Albion;
using Serilog;
using Serilog.Events;

var builder = Host.CreateDefaultBuilder(args)
    .UseSerilog((context, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services).Enrich.FromLogContext().WriteTo.Console();
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });

var app = builder.Build();
if (AlbionWorldData.GetDataListFromJson())
    await app.RunAsync();
