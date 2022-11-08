
; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

; Lightning Setup Script
; August 11, 2022
; Updated August 12, 2022 for relative paths
; Updated September 25, 2022 for Lightning 1.1
; Updated October 17, 2022 for Lightning 1.1 (again)
; Updated October 21, 2022 to add an Animation Editor shortcut
; Intended to be run from the SDKBuild tool ONLY.

#define MyAppName "Lightning Software Development Kit"                                                     
#define MyAppVersion "1.1.3"
#define MyAppPublisher "starfrost"
#define MyAppURL "https://lightning.starfrost.net"
#define BuildConfig "Debug"

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
OutputDir=..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\Setup
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

Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\libFLAC-8.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\libmodplug-1.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\libmpg123-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\libogg-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\libopus-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\libopusfile-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\libpng16-16.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\libvorbis-0.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\libvorbisfile-3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\LightningBase.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\LightningGL.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\LightningGL.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\LightningPackager.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\AnimTool.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\AnimTool.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\AnimTool.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\AnimTool.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\MakePackage.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\MakePackage.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\MakePackage.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\MakePackage.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\NuCore.Utilities.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\NuCore.Utilities.Lightning.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\zlib1.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\Content\*"; DestDir: "{app}\Content"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\Documentation\*"; DestDir: "{app}\Documentation"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\Examples\*"; DestDir: "{app}\Examples"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\Libraries\*"; DestDir: "{app}\Libraries"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\SDKBuild\bin\{#BuildConfig}\net6.0\SDK\VSTemplate\*"; DestDir: "{userdocs}\Visual Studio 2022\Templates\ProjectTemplates"; 
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\Lightning Documentation"; Filename: "{win}\explorer.exe"; Parameters: "{app}\Documentation";
Name: "{group}\Lightning Examples"; Filename: "{win}\explorer.exe"; Parameters: "{app}\Examples";
Name: "{group}\Lightning Animation Editor"; Filename: "{app}\AnimTool.exe";

;Name: "{group}\Get Started"; Filename:"explorer {app}\Examples";
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"

