
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
; Updated March 21, 2023 to yank out unfinished stuff for 2.0 submission
; Updated March 28, 2023 to fix scene template installation

#define MyAppName "Lightning Software Development Kit"                                                     
#define MyAppVersion "2.0.0-rc1"
#define MyAppPublisher "starfrost"
#define MyAppURL "https://lightningpowered.net"
#define BuildConfig "Debug"   ; for sdkbuild
#define FrameworkVersion "net7.0"

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
ArchitecturesInstallIn64BitMode=x64
ArchitecturesAllowed=x64
; icon
SetupIconFile = ..\Icons\Lightning.ico
DisableDirPage = yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
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
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\LightningUtil.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\LightningUtilSdl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\SDL2.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\SDL2_image.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\SDL2_mixer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\zlib1.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\Content\*"; DestDir: "{app}\Content"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\Documentation\*"; DestDir: "{app}\Documentation"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\Examples\*"; DestDir: "{app}\Examples"; Flags: ignoreversion recursesubdirs createallsubdirs

Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\VSTemplate\Lightning Game Project.zip"; DestDir: "{userdocs}\Visual Studio 2022\Templates\ProjectTemplates"; 
Source: "..\SDKBuild\bin\{#BuildConfig}\{#FrameworkVersion}\SDK\VSTemplate\Lightning Scene Template.zip"; DestDir: "{userdocs}\Visual Studio 2022\Templates\ItemTemplates"; 

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\Lightning Documentation"; Filename: "{win}\explorer.exe"; Parameters: "{app}\Documentation";
Name: "{group}\Lightning Examples"; Filename: "{win}\explorer.exe"; Parameters: "{app}\Examples";
Name: "{group}\Lightning Animation Editor"; Filename: "{app}\AnimTool.exe";

;Name: "{group}\Get Started"; Filename:"explorer {app}\Examples";
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
