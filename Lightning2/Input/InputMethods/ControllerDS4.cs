

namespace LightningGL
{
    /// <summary>
    /// Detection code for the Sony PS4 DualShock 4 controller.
    /// </summary>
    internal class ControllerDS4 : ControllerXinput
    {
        internal override bool DetectPresence()
        {
            if (!base.DetectPresence()) return false;

            NCLogging.Log("Detecting DualShock 4...", "Device Detection");

            if ((KnownControllerVendorIds)VendorID == KnownControllerVendorIds.Sony 
                && ((KnownControllerProductIds)ProductID == KnownControllerProductIds.DualShock4Gen1
                || (KnownControllerProductIds)ProductID == KnownControllerProductIds.DualShock4Gen2
                || (KnownControllerProductIds)ProductID == KnownControllerProductIds.DualShock4WirelessAdaptor) // wireless
                )
            {
                NCLogging.Log("Detected DualShock 4");
                return true;
            }

            return false;
        }
    }
}
