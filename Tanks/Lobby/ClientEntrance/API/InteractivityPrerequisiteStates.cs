namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class InteractivityPrerequisiteStates
    {
        public class AcceptableState : Node
        {
            public AcceptableStateComponent acceptableState;
            public InteractivityPrerequisiteStateComponent interactivityPrerequisiteState;
        }

        public class NotAcceptableState : Node
        {
            public NotAcceptableStateComponent notAcceptableState;
            public InteractivityPrerequisiteStateComponent interactivityPrerequisiteState;
        }
    }
}

