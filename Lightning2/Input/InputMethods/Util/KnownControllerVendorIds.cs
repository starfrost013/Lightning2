
namespace LightningGL
{
    /// <summary>
    /// KnownControlerVendorIds
    /// 
    /// Known controller vendor IDs.
    /// Simply used for cleaner code and detecting specific game controllers.
    /// </summary>
    internal enum KnownControllerVendorIds : ushort
    {
        Microsoft = 0x045E,

        Logitech = 0x046D,

        Sony = 0x054C,

        Nintendo = 0x057E,

        PadixRockfire = 0x0583,

        MadCatz = 0x0738,

        SteelSeries = 0x1038,

        Razer = 0x1532,

        EightBitDo = 0x2dc8,

        Hyperkin = 0x2e24,
    }
}