namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ServiceMessageStates
    {
        public class ServiceMessageHiddenState : Node
        {
            public ServiceMessageHiddenStateComponent serviceMessageHiddenState;
        }

        public class ServiceMessageVisibleState : Node
        {
            public ServiceMessageVisibleStateComponent serviceMessageVisibleState;
        }
    }
}

