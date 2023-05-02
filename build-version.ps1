$registry="ghcr.io"
$repository="ridomin/mqtttester"
$version=(nbgv get-version -v NuGetPackageVersion)
dotnet publish /t:PublishContainer .\MqttTester\ --os linux --arch x64 /p:ContainerImageTags=$version /p:ContainerRegistry=$registry /p:ContainerRepository=$repository 
```