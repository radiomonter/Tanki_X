namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class HangarCameraRotationState
    {
        public class Disabled : Node
        {
            public HangarCameraRotationDisabledComponent hangarCameraRotationDisabled;
        }

        public class Enabled : Node
        {
            public HangarCameraRotationEnabledComponent hangarCameraRotationEnabled;
        }
    }
}

