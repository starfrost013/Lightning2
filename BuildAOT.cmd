@echo off

echo Building Lightning for NativeAOT (config %1, architecture %2, output dir %3)
dotnet publish LightningGL\LightningGL.csproj -c %1 -r %2 -p:DefineConstants=AOT -o %3