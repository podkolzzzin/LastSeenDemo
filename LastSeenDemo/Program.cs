using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LastSeenDemo;
using System.Net.Http;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient<ILoader, Loader>();
        services.AddSingleton<IUserLoader>(provider =>
            new UserLoader(provider.GetRequiredService<ILoader>(), "https://sef.podkolzin.consulting/api/users/lastSeen"));
        services.AddSingleton<ILastSeenApplication, LastSeenApplication>();
    })
    .Build();

var application = host.Services.GetRequiredService<ILastSeenApplication>();
var result = application.Show(DateTimeOffset.Now);

foreach (var item in result)
    Console.WriteLine(item);
