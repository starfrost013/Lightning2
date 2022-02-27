using NuCore.SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCMessageBoxButton
    /// 
    /// February 27, 2022
    /// 
    /// Defines an NCMessageBoxButton
    /// </summary>
    public class NCMessageBoxButton
    {
        public string Text { get; set; }
        public SDL.SDL_MessageBoxButtonFlags Flags { get; set; }
        public int ID { get; internal set; }


        public static explicit operator SDL.SDL_MessageBoxButtonData(NCMessageBoxButton Button)
        {
            return new SDL.SDL_MessageBoxButtonData
            {
                buttonid = Button.ID,
                flags = Button.Flags,
                text = Button.Text
                // todo: other stuff
            };

        }
    }
}
