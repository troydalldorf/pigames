dotnet publish -r linux-arm -c Release --self-contained
cd .All/bin/Release/net7.0/linux-arm/publish || exit
chmod +x All