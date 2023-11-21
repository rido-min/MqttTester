using Microsoft.Azure.IoTMQ.IoTHubConnector.Client.Connection.Settings;
using Microsoft.Azure.IoTMQ.IoTHubConnector.Client.Connection;
using Microsoft.Azure.IoTMQ.IoTHubConnector.Client;
using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using MQTTnet;

namespace MqttTester
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("\nReading Mqtt Connection Settings from '--ConnectionStrings:Default'");
            _logger.LogInformation("\nPackageVersions \n\nMqttTester: {t}\nIotMQClient: {n}\nMqttNet: {m}\n",
                ThisAssembly.AssemblyFileVersion,
                IotHubClient.GetSdkVersion(),
                typeof(MqttClient).Assembly.GetName().Version!.ToString());


            var cs = MqttConnectionSettings.FromConnectionString(_config.GetConnectionString("Default")!);

            _logger.LogInformation("Connecting with: {cs}", cs);

            IMqttClient mqttClient = new MqttFactory().CreateMqttClient(MqttNetTraceLogger.CreateTraceLogger());
            await mqttClient.ConnectAsync(new MqttClientOptionsBuilder().WithMqttConnectionSettings(cs).Build(), stoppingToken);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    Console.Write(".");
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
