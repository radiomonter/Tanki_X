namespace Tanks.ClientLauncher.Impl
{
    using System;
    using System.Runtime.CompilerServices;

    public class UpdateConfiguration
    {
        public string LastClientVersion { get; set; }

        public string DistributionUrl { get; set; }

        public string Executable { get; set; }

        public static UpdateConfiguration Config { get; set; }
    }
}

