:: cmd to make web package for iis deploy

dotnet build web.csproj /nologo /p:PublishProfile=Release /p:PackageLocation="..\web_package" /p:OutDir="..\web_package\out" /p:DeployOnBuild=true /p:WebPublishMethod=Package /p:PackageAsSingleFile=true /maxcpucount:1 /p:platform="Any CPU" /p:configuration="Release" /p:DesktopBuildPackageLocation="..\package.zip