using static NuCore.SDL2.SDL;

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
        public SDL_MessageBoxButtonFlags Flags { get; set; }
        public int ID { get; internal set; }

        public static explicit operator SDL_MessageBoxButtonData(NCMessageBoxButton Button)
        {
            return new SDL_MessageBoxButtonData
            {
                buttonid = Button.ID,
                flags = Button.Flags,
                text = Button.Text
                // todo: other stuff
            };

        }
    }
}
