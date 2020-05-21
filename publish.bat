dotnet publish -o publish-win -r win-x64 -c release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true nats-tools.csproj 
dotnet publish -o publish-linux -r linux-x64 -c release --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true nats-tools.csproj
