using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuCore.Utilities
{
    /// <summary>
    /// NCMessageBoxColourScheme
    /// 
    /// February 27, 2022
    /// 
    /// Defines an NCMessageBoxColourScheme,
    /// five colours to be used for NC Messageboxes.
    /// </summary>
    public class NCMessageBoxColourScheme
    {
        /// <summary>
        /// The background colour in the five-colour colour scheme.
        /// The alpha component is always ignored.
        /// </summary>
        Color4 BackgroundColour { get; set; }

        /// <summary>
        /// The text colour in the five-colour colour scheme.
        /// The alpha component is always ignored.
        /// </summary
        Color4 TextColour { get; set; }

        /// <summary>
        /// The button border colour in the five-colour colour scheme.
        /// The alpha component is always ignored.
        /// </summary
        Color4 ButtonBorderColour { get; set; }

        /// <summary>
        /// The button background colour in the five-colour colour scheme.
        /// The alpha component is always ignored.
        /// </summary
        Color4 ButtonBackgroundColour { get; set; }

        /// <summary>
        /// The button selected colour in the five-colour colour scheme.
        /// The alpha component is always ignored.
        /// </summary
        Color4 ButtonSelectedColour { get; set; }

        public NCMessageBoxColourScheme()
        {
            // default colour scheme
            BackgroundColour = new Color4(255, 255, 255, 255);
            TextColour = new Color4(0, 0, 0, 255);
            ButtonBorderColour = new Color4(0, 0, 0, 255);
            ButtonBackgroundColour = new Color4(127, 127, 192, 255);
            ButtonSelectedColour = new Color4(127, 127, 127, 255);
        }
    }
}
