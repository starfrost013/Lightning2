
namespace LightningBase
{
    internal static class SdlMarshaling
    {
        #region Marshaling

        internal static T PtrToStructure<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>
            (IntPtr ptr)
        {
            return Marshal.PtrToStructure<T>(ptr);
        }

        internal static T GetDelegateForFunctionPointer<T>(IntPtr ptr) where T : Delegate
        {
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        internal static int SizeOf<T>()
        {
            return Marshal.SizeOf<T>();
        }
    }
}
