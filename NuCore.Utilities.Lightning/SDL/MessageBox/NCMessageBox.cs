using System.Collections.Generic;
using System.Diagnostics;
using static LightningBase.SDL;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCMessageBox
    /// 
    /// February 26, 2022 (updated August 10, 2022) 
    /// 
    /// Platform-independent API for message boxes
    /// </summary>
    public class NCMessageBox
    {
        /// <summary>
        /// Icon of the message box.
        /// </summary>
        public SDL_MessageBoxFlags Icon { get; set; }

        /// <summary>
        /// The title to use for the message box window.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// The text to use in the message box wiodow,
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// The buttons that are to be used in this message box.
        /// </summary>
        private List<NCMessageBoxButton> Buttons { get; set; }

        /// <summary>
        /// Constructor for <see cref="NCMessageBox"/>
        /// </summary>
        public NCMessageBox()
        {
            Buttons = new List<NCMessageBoxButton>();
        }

        /// <summary>
        /// Adds a button to this NCMessageBox.
        /// </summary>
        /// <param name="text">The text of this message box button.</param>
        /// <param name="flags">The flags - see <see cref="SDL.SDL_MessageBoxButtonFlags"/>.</param>
        public void AddButton(string text, SDL_MessageBoxButtonFlags flags = 0)
        {
            Buttons.Add(new NCMessageBoxButton
            {
                Flags = flags,
                ID = Buttons.Count,
                Text = text,
            });
        }

        /// <summary>
        /// Shows this message box.
        /// </summary>
        /// <returns>A value indicating if this message box was returned or not.</returns>
        public NCMessageBoxButton? Show()
        {
            // Create a new list of button data.
            List<SDL_MessageBoxButtonData> buttonData = new List<SDL_MessageBoxButtonData>();

            foreach (NCMessageBoxButton button in Buttons)
            {
                // use the explicit operator to convert
                buttonData.Add((SDL_MessageBoxButtonData)button);
            }

            // build the message box data
            SDL_MessageBoxButtonData[] buttonArray = buttonData.ToArray();

            SDL_MessageBoxData mbData = new SDL_MessageBoxData();

            // parent window currently not supported
            mbData.buttons = buttonArray;
            mbData.numbuttons = buttonArray.Length;
            mbData.title = Title;
            mbData.message = Text;
            mbData.flags = Icon;

            // Show the message box
            if (SDL_ShowMessageBox(ref mbData, out var buttonId) < 0) _ = new NCException($"Error creating messagebox - {SDL_GetError()}", 19, "SDL2 error occurred in NCMessageBox::Show", NCExceptionSeverity.FatalError);

            Debug.Assert(buttonId < buttonArray.Length);

            if (buttonId == -1) // No button selected
            {
                return null;
            }
            else
            {
                return Buttons[buttonId];
            }

        }
    }
}
