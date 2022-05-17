// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Subscriber;


public class Program
{
    public static async Task Main()
    {
        await new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ServiceRabbitMq>(); // register our service here            
            })
            .RunConsoleAsync();
    }
}