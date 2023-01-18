#region License
/* Lightning SDL2 Wrapper
 * 
 * Version 3.1.0
 * Copyright © 2022 starfrost
 * August 31, 2022
 * 
 * This software is based on the open-source SDL2# - C# Wrapper for SDL2 library.
 *
 * Copyright © 2013-2021 Ethan Lee.
 * Copyright © 2022 starfrost
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from
 * the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 * claim that you wrote the original software. If you use this software in a
 * product, an acknowledgment in the product documentation would be
 * appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source distribution.
 *
 * Ethan "flibitijibibo" Lee <flibitijibibo@flibitijibibo.com>
 *
 */
#endregion

#region Using Statements
#endregion

namespace LightningBase
{
    public static partial class SDL
    {
        #region SDL_blendmode.h

        [Flags]
        public enum SDL_BlendMode
        {
            SDL_BLENDMODE_NONE = 0x00000000,
            SDL_BLENDMODE_BLEND = 0x00000001,
            SDL_BLENDMODE_ADD = 0x00000002,
            SDL_BLENDMODE_MOD = 0x00000004,
            SDL_BLENDMODE_MUL = 0x00000008, /* >= 2.0.11 */
            SDL_BLENDMODE_INVALID = 0x7FFFFFFF
        }

        public enum SDL_BlendOperation
        {
            SDL_BLENDOPERATION_ADD = 0x1,
            SDL_BLENDOPERATION_SUBTRACT = 0x2,
            SDL_BLENDOPERATION_REV_SUBTRACT = 0x3,
            SDL_BLENDOPERATION_MINIMUM = 0x4,
            SDL_BLENDOPERATION_MAXIMUM = 0x5
        }

        public enum SDL_BlendFactor
        {
            SDL_BLENDFACTOR_ZERO = 0x1,
            SDL_BLENDFACTOR_ONE = 0x2,
            SDL_BLENDFACTOR_SRC_COLOR = 0x3,
            SDL_BLENDFACTOR_ONE_MINUS_SRC_COLOR = 0x4,
            SDL_BLENDFACTOR_SRC_ALPHA = 0x5,
            SDL_BLENDFACTOR_ONE_MINUS_SRC_ALPHA = 0x6,
            SDL_BLENDFACTOR_DST_COLOR = 0x7,
            SDL_BLENDFACTOR_ONE_MINUS_DST_COLOR = 0x8,
            SDL_BLENDFACTOR_DST_ALPHA = 0x9,
            SDL_BLENDFACTOR_ONE_MINUS_DST_ALPHA = 0xA
        }

        /* Only available in 2.0.6 or higher. */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_BlendMode SDL_ComposeCustomBlendMode(
            SDL_BlendFactor srcColorFactor,
            SDL_BlendFactor dstColorFactor,
            SDL_BlendOperation colorOperation,
            SDL_BlendFactor srcAlphaFactor,
            SDL_BlendFactor dstAlphaFactor,
            SDL_BlendOperation alphaOperation
        );

        #endregion
    }
}
