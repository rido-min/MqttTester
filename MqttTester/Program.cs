using MqttTester;
using System.Diagnostics;

internal class Program
{
    public static void Main(string[] args)
    {
        Trace.Listeners.Add(new ConsoleTraceListener());
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        host.Run();
    }
}