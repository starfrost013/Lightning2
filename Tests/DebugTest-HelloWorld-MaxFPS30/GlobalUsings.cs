// Lightning Global Using file for Example Project
// January 29, 2023

// DO NOT MODIFY unless you know what you are doing

// Lightning itself
global using LightningBase;
global using LightningGL;
global using LightningPackager; // remove if you don't want the packager
// Core .NET stuff lightning uses a lot
global using System.Drawing;
global using System.Numerics;
// You shouldn't need this, but it still might be helpful (FreeType bindings)
global using static LightningBase.FreeTypeApi;
// SDL2 stuff (to make accessing SDL functions less painful)
global using static LightningBase.SDL;
global using static LightningBase.SDL_image;
global using static LightningBase.SDL_mixer;
global using static LightningGL.Lightning;
