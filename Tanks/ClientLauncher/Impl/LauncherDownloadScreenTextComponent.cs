namespace Tanks.ClientLauncher.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LauncherDownloadScreenTextComponent : Component
    {
        public string DownloadText { get; set; }

        public string RebootText { get; set; }
    }
}

