echo off
md artifacts\staging\lib\net45
echo 'staging'
copy Utils.Core\bin\Release\Utils.Core.dll artifacts\staging\lib\net45  
copy Utils.Core\Utils.Core.nuspec artifacts\staging
toolset\nuget\nuget pack -version 1.0.0 Utils.Core\Utils.Core.csproj -IncludeReferencedProjects -OutputDirectory artifacts\packages -Properties Configuration=Release
