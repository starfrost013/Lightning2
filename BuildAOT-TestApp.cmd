@echo off

echo Building Lightning Test App for NativeAOT (config %1, architecture %2, output dir %3)
dotnet publish LightningGL.Test\LightningGL.Test.csproj -c %1 -r %2 -p:DefineConstants=AOT -o %3