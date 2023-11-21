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
            var cs = MqttConnectionSettings.FromConnectionString(_config.GetConnectionString("Default")!);
            _logger.LogWarning("Connecting with: {cs}", cs);
            _logger.LogWarning("Nuget: {n}", IotHubClient.GetSdkVersion());
            IMqttClient mqttClient = new MqttFactory().CreateMqttClient(MqttNetTraceLogger.CreateTraceLogger());
            await mqttClient.ConnectAsync(new MqttClientOptionsBuilder().WithMqttConnectionSettings(cs).Build(), stoppingToken);
            _logger.LogWarning("Client Connected: {c}", mqttClient.IsConnected);
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
