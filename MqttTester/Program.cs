using MqttTester;
using System.Diagnostics;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services
            .AddSingleton(MqttSessionClientFactoryProvider.MqttSessionClientFactory)
            .AddHostedService<Worker>();

        var host = builder.Build();
        host.Run();
    }
}