mkdir ../publish
dotnet build ../src/StonkBot.csproj -c Release -r win10-x64 --self-contained false --output ../publish/StonkBot_Windows
dotnet build ../src/StonkBot.csproj -c Release -r win10-x64 --self-contained true --output ../publish/StonkBot_Windows_Portable
dotnet build ../src/StonkBot.csproj -c Release -r linux-x64 --self-contained false --output ../publish/StonkBot_Linux
dotnet build ../src/StonkBot.csproj -c Release -r linux-x64 --self-contained true --output ../publish/StonkBot_Linux_Portable