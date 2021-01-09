namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class HangarCameraState
    {
        public class Disabled : Node
        {
            public HangarCameraDisabledComponent hangarCameraDisabled;
        }

        public class Enabled : Node
        {
            public HangarCameraEnabledComponent hangarCameraEnabled;
        }
    }
}

