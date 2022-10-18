using static LightningBase.SDL;

namespace LightningBase
{
    public static class LightningBase
    {
        /// <summary>
        /// The version of the NuCore SDL2 Bindings
        /// </summary>
        internal static string BASE_VERSION = $"Lightning Base version {BASE_VERSION_MAJOR}.{BASE_VERSION_MINOR}.{BASE_VERSION_REVISION} " +
            $"(Expected SDL version {SDL_EXPECTED_MAJOR_VERSION}.{SDL_EXPECTED_MINOR_VERSION}.{SDL_EXPECTED_PATCHLEVEL})"; // cannot be const

        private const int BASE_VERSION_MAJOR = 3;
        private const int BASE_VERSION_MINOR = 1;
        private const int BASE_VERSION_REVISION = 1;
    }
}
