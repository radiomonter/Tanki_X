namespace Assets.platform.library.ClientResources.Scripts.API
{
    using System;
    using UnityEngine;

    public static class DiskCaching
    {
        private static bool enabled = Caching.enabled;

        public static bool Enabled
        {
            get => 
                enabled;
            set => 
                enabled = value;
        }

        public static long MaximumAvailableDiskSpace
        {
            set => 
                Caching.maximumAvailableDiskSpace = value;
        }

        public static int ExpirationDelay
        {
            set => 
                Caching.expirationDelay = value;
        }

        public static bool CompressionEnambled
        {
            set => 
                Caching.compressionEnabled = value;
        }
    }
}

