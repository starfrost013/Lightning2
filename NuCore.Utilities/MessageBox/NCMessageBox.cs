using NuCore.SDL2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCMessageBox
    /// 
    /// February 26, 2022 (updated April 15, 2022) 
    /// 
    /// Platform-independent API for message boxes
    /// </summary>
    public class NCMessageBox
    {
        public SDL.SDL_MessageBoxFlags Icon { get; set; }

        public string Title { get; set; }  

        public string Message { get; set; }

        private List<NCMessageBoxButton> Buttons { get; set; }

        public NCMessageBox()
        {
            Buttons = new List<NCMessageBoxButton>();
        }

        /// <summary>
        /// Adds a button to this NCMessageBox.
        /// </summary>
        /// <param name="Name">The text of this message box button.</param>
        /// <param name="Flags">The flags - see </param>
        public void AddButton(string Text, SDL.SDL_MessageBoxButtonFlags Flags = 0)
        {
            Buttons.Add(new NCMessageBoxButton
            {
                Flags = Flags,
                ID = Buttons.Count,
                Text = Text,
            });
        }

        public NCMessageBoxButton Show()
        {
            // Create a new list of button data.
            List<SDL.SDL_MessageBoxButtonData> button_data = new List<SDL.SDL_MessageBoxButtonData>();

            foreach (NCMessageBoxButton button in Buttons)
            {
                // use the explicit operator to convert
                button_data.Add((SDL.SDL_MessageBoxButtonData)button);
            }

            SDL.SDL_MessageBoxButtonData[] button_array = button_data.ToArray();

            SDL.SDL_MessageBoxData mb_data = new SDL.SDL_MessageBoxData();

            // parent window currently not supported
            mb_data.buttons = button_array;
            mb_data.numbuttons = button_array.Length;
            mb_data.title = Title;
            mb_data.message = Message;
            mb_data.flags = Icon;

            int button_id;

            if (SDL.SDL_ShowMessageBox(ref mb_data, out button_id) < 0) throw new NCException($"Error creating messagebox - {SDL.SDL_GetError}", 19, "NCMessageBox.Show()", NCExceptionSeverity.FatalError);

            Debug.Assert(button_id < button_array.Length);
        
            if (button_id == -1) // No button selected
            {
                return null; 
            }
            else
            {
                return Buttons[button_id];
            }
             
        }
    }
}
