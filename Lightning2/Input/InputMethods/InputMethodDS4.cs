

namespace LightningGL
{
    /// <summary>
    /// Detection code for the Sony PS4 DualShock 4 controller.
    /// </summary>
    internal class InputMethodDS4 : InputMethodXinput
    {
        internal override bool DetectPresence()
        {
            NCLogging.Log("Detecting DualShock 4...");

            if (!base.DetectPresence())
            {
                NCLogging.Log("DualShock 4 not present");
                return false;
            }
            
            if ((KnownControllerVendorIds)VendorID == KnownControllerVendorIds.Sony 
                && ((KnownControllerProductIds)ProductID == KnownControllerProductIds.DualShock4Gen1
                || (KnownControllerProductIds)ProductID == KnownControllerProductIds.DualShock4Gen2
                || (KnownControllerProductIds)ProductID == KnownControllerProductIds.DualShock4WirelessAdaptor) // wireless
                )
            {
                NCLogging.Log("Detected DualShock 4!");
                return true;
            }


            return false;
        }
    }
}
