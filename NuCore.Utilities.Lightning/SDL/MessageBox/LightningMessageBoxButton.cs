using static LightningBase.SDL;

namespace LightningUtil
{
    /// <summary>
    /// NCMessageBoxButton
    /// 
    /// February 27, 2022
    /// 
    /// Defines an NCMessageBoxButton
    /// </summary>
    public class LightningMessageBoxButton
    {
        public string? Text { get; set; }
        public SDL_MessageBoxButtonFlags Flags { get; set; }
        public int ID { get; internal set; }

        public static explicit operator SDL_MessageBoxButtonData(LightningMessageBoxButton Button)
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
