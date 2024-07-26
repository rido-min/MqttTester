using Akri.Mqtt.Connection;
using Akri.Mqtt.Session;
using MQTTnet.Client;

namespace MqttTester;

public class Worker(MqttSessionClient mqttClient, ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("\nReading Mqtt Connection Settings from '--ConnectionStrings:Default'");
        logger.LogInformation("\nPackageVersions \n\nMqttTester: {t}\nAkri.Mqtt: {n}\nMqttNet: {m}\n",
            ThisAssembly.AssemblyFileVersion,
            typeof(MqttSessionClient).Assembly.GetName().Version!.ToString(),
            typeof(MqttClient).Assembly.GetName().Version!.ToString());

        var cs = MqttConnectionSettings.FromConnectionString(configuration.GetConnectionString("Default")!);

        logger.LogInformation("Connecting with: {cs}", cs);
        var connAck  = await mqttClient.ConnectAsync(cs, stoppingToken);
        logger.LogInformation("ConnAck resultCode: {connAck}", connAck.ResultCode);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                Console.Write(".");
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
