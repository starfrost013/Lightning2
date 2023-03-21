// Lightning Global Using file for Example Projectss
// October 21, 2022 (modified March 21, 2023)

// DO NOT MODIFY unless you know what you are doing

// Lightning itself
global using LightningBase;
global using LightningGL;
global using LightningPackager; // remove if you don't want the packager

// Core .NET stuff lightning uses a lot
global using Color = System.Drawing.Color;
global using Rectangle = LightningGL.Rectangle;
global using System.Numerics;
// SDL2 stuff (to make accessing SDL functions less painful)
global using static LightningBase.SDL;

global using static LightningBase.SDL_image;
global using static LightningBase.SDL_mixer;

global using static LightningGL.Lightning;
