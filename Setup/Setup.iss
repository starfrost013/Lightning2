
; Lightning Setup Script
; August 11, 2022
; Intended to be run from the SDKBuild tool ONLY!

; Updated August 12, 2022 for relative paths
; Updated September 25, 2022 for Lightning 1.1
; Updated October 17, 2022 for Lightning 1.1 (again)
; Updated October 21, 2022 to add an Animation Editor shortcut
; Updated December 3, 2022 for .NET 7.0
; Updated December 17, 2022 for Lightning 2.0 file locations
; Updated January 2, 2023 for no more SDL_ttf/gfx
; Updated January 29, 2023 for installing prereqs
; Updated February 9, 2023 to actually install prereqs

#define MyAppName "Lightning Software Development Kit"                                                     
#define MyAppVersion "2.0.0-alpha"
#define MyAppPublisher "starfrost"
#define MyAppURL "https://lightningpowered.net"
#define BuildConfig "Debug"
#define FrameworkVersion "net7.0"

; Workaround for the inno download installer not actually adding itself to ISPPBuiltins on inno6
#include ReadReg(HKLM, 'Software\WOW6432Node\Mitrich Software\Inno Download Plugin', 'InstallDir') + '\idp.iss'

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{5EC660FC-A60A-4635-93DF-652F2A70341C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=commandline
OutputDir=..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\Setup
OutputBaseFilename=SDKSetup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
; we are 64-bit only
ArchitecturesInstallIn64BitMode=x64 arm64
ArchitecturesAllowed=x64 arm64
; icon
SetupIconFile = ..\Icons\Lightning.ico
DisableDirPage = yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\InstallHelper.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libFLAC-8.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libmodplug-1.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libmpg123-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libogg-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libopus-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libopusfile-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libpng16-16.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libvorbis-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libvorbisfile-3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libtiff-5.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libwebp-7.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\libjpeg-5.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\LightningBase.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\LightningGL.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\LightningGL.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\LightningPackager.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\AnimTool.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\AnimTool.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\AnimTool.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\AnimTool.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\freetype.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\MakePackage.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\MakePackage.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\MakePackage.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\MakePackage.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\NuCore.Utilities.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\NuCore.Utilities.Lightning.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\SDL2.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\SDL2_image.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\SDL2_mixer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\zlib1.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\Content\*"; DestDir: "{app}\Content"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\Documentation\*"; DestDir: "{app}\Documentation"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\Examples\*"; DestDir: "{app}\Examples"; Flags: ignoreversion recursesubdirs createallsubdirs

Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\VSTemplate\*"; DestDir: "{userdocs}\Visual Studio 2022\Templates\ProjectTemplates"; 
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\Lightning Documentation"; Filename: "{win}\explorer.exe"; Parameters: "{app}\Documentation";
Name: "{group}\Lightning Examples"; Filename: "{win}\explorer.exe"; Parameters: "{app}\Examples";
Name: "{group}\Lightning Animation Editor"; Filename: "{app}\AnimTool.exe";

;Name: "{group}\Get Started"; Filename:"explorer {app}\Examples";
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"

[CustomMessages]
IDP_DownloadFailed=Download of .NET 7.0 SDK failed. The .NET 7.0 SDK is required to run the Lightning Game Engine SDK.
IDP_RetryCancel=Click 'Retry' to try downloading the files again, or click 'Cancel' to terminate setup.
InstallingDotNetSDK=Installing the .NET 7.0 SDK. This may take a few minutes.
DotNetSDKFailedToLaunch=Failed to launch .NET SDK installer with error "%1". Please fix the error, if applicable, then run this installer again.
DotNetSDKFailed1602=The .NET SDK installation was cancelled. You must install the .NET 7 SDK to use the Lightning Game Engine SDK. Please restart the installer.
DotNetSDKFailed1603=A fatal error occurred while installing the .NET 7 SDK. Please fix the oribkem, if applicable. run the installer again.
DotNetSDKFailed5100=Your computer does not meet the requirements of the .NET SDK. Installation cannot continue.
DotNetSDKFailedOther=The .NET SDK installer exited with an unexpected status code "%1". Please review any other messages shown by the installer to determine whether the installation completed successfully, and abort this installation and fix the problem if it did not.

[Code]
{why would you choose pascal as a scripting language}
{code stolen from https://engy.us/blog/2021/02/28/installing-net-5-runtime-automatically-with-inno-setup/}
function IsNet7Installed() : Boolean;
external 'IsNet7Installed@files:InstallHelper.dll';


var
  requiresRestart: boolean;

procedure InitializeWizard;
begin
  if IsNet7Installed() = False then
  begin
    idpAddFile('https://download.visualstudio.microsoft.com/download/pr/6ba69569-ee5e-460e-afd8-79ae3cd4617b/16a385a4fab2c5806f50f49f5581b4fd/dotnet-sdk-7.0.102-win-x64.exe', ExpandConstant('{tmp}\NetSdkInstaller.exe'));
    idpDownloadAfter(wpReady);
  end;
end;

function InstallDotNetRuntime(): String;
var
  StatusText: string;
  ResultCode: Integer;
begin
  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := CustomMessage('InstallingDotNetSDK');
  WizardForm.ProgressGauge.Style := npbstMarquee;
  try
    if not Exec(ExpandConstant('{tmp}\NetSdkInstaller.exe'), '/passive /norestart /showrmui /showfinalerror', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
    begin
      Result := FmtMessage(CustomMessage('DotNetSDKFailedToLaunch'), [SysErrorMessage(resultCode)]);
    end
    else
    begin
      // See https://msdn.microsoft.com/en-us/library/ee942965(v=vs.110).aspx#return_codes
      case resultCode of
        0: begin
          // Successful
        end;
        1602 : begin
          Result := CustomMessage('DotNetSDKFailed1602');
        end;
        1603: begin
          Result := CustomMessage('DotNetSDKFailed1603');
        end;
        1641: begin
          requiresRestart := True;
        end;
        3010: begin
          requiresRestart := True;
        end;
        5100: begin
          Result := CustomMessage('DotNetSDKFailed5100');
        end;
        else begin
          MsgBox(FmtMessage(CustomMessage('DotNetSDKFailedOther'), [IntToStr(resultCode)]), mbError, MB_OK);
        end;
      end;
    end;
  finally
    WizardForm.StatusLabel.Caption := StatusText;
    WizardForm.ProgressGauge.Style := npbstNormal;
    
    DeleteFile(ExpandConstant('{tmp}\NetRuntimeInstaller.exe'));
  end;
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
  // 'NeedsRestart' only has an effect if we return a non-empty string, thus aborting the installation.
  // If the installers indicate that they want a restart, this should be done at the end of installation.
  // Therefore we set the global 'restartRequired' if a restart is needed, and return this from NeedRestart()

  if IsNet7Installed() = False then
  begin
    Result := InstallDotNetRuntime();
  end;
end;

function NeedRestart(): Boolean;
begin
  Result := requiresRestart;
end;
