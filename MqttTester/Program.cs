using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet.Client;
using MQTTnet.Extensions.MultiCloud.AzureIoTClient;
using MQTTnet.Extensions.MultiCloud.BrokerIoTClient;
using MQTTnet.Extensions.MultiCloud.Connections;
using System.Diagnostics;

Trace.Listeners.Add(new ConsoleTraceListener());

using IHost host = Host.CreateDefaultBuilder(args).Build();
host.Start();
IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
ILogger logger = host.Services.GetRequiredService<ILogger<Program>>();

var cs = new ConnectionSettings(config.GetConnectionString("cs")!);
logger.LogWarning("Connecting with: {cs}", cs);
IMqttClient mqttClient;

if (!string.IsNullOrEmpty(cs.IdScope) || cs.HostName!.Contains("azure-devices.net"))
{
    logger.LogWarning("Nuget: {n}", HubDpsFactory.NuGetPackageVersion);
    mqttClient = await HubDpsFactory.CreateFromConnectionSettingsAsync(cs);
}
else
{
    logger.LogWarning("Nuget: {n}", BrokerClientFactory.NuGetPackageVersion);
    mqttClient = await BrokerClientFactory.CreateFromConnectionSettingsAsync(cs);
}
logger.LogWarning("Client Connected: {c}", mqttClient.IsConnected);