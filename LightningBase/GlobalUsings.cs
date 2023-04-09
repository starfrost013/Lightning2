// This file provides global using statements.
// Don't put any usings in individual source files, put any you need to use in here.

#region Global using statements
global using LightningUtil;
global using System;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.IO;
global using System.Reflection;
global using System.Runtime.InteropServices;
#if X64
global using System.Runtime.Intrinsics.X86;
#else
global using System.Runtime.Intrinsics.Arm;
#endif
global using System.Runtime.Versioning;
global using System.Text;
global using static LightningBase.SDL;
global using static LightningBase.SDL_mixer;
#endregion