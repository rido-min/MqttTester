$version=(nbgv get-version -v NuGetPackageVersion)
dotnet pack MqttTester\MqttTester.csproj
dotnet publish /t:PublishContainer .\MqttTester\ --os linux --arch x64 /p:ContainerImageTags=$version /p:ContainerRegistry="ghcr.io" /p:ContainerRepository="ridomin/mqtttester"