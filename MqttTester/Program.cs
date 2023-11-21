using Microsoft.Azure.IoTMQ.IoTHubConnector.Client;
using Microsoft.Azure.IoTMQ.IoTHubConnector.Client.Connection;
using Microsoft.Azure.IoTMQ.IoTHubConnector.Client.Connection.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using System.Diagnostics;

Trace.Listeners.Add(new ConsoleTraceListener());

using IHost host = Host.CreateDefaultBuilder(args).Build();
host.Start();

IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
ILogger logger = host.Services.GetRequiredService<ILogger<Program>>();

var cs = MqttConnectionSettings.FromConnectionString(config.GetConnectionString("Default")!);
logger.LogWarning("Connecting with: {cs}", cs);
logger.LogWarning("Nuget: {n}", DeviceClient.GetSdkVersion());
IMqttClient mqttClient = new MqttFactory().CreateMqttClient(MqttNetTraceLogger.CreateTraceLogger());
await mqttClient.ConnectAsync(new MqttClientOptionsBuilder().WithMqttConnectionSettings(cs).Build());
logger.LogWarning("Client Connected: {c}", mqttClient.IsConnected);