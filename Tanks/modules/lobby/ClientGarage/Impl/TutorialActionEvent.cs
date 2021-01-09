namespace tanks.modules.lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15ea8c47e42L)]
    public class TutorialActionEvent : Event
    {
        public TutorialActionEvent()
        {
        }

        public TutorialActionEvent(long tutorialId, long stepId, TutorialAction action)
        {
            this.TutorialId = tutorialId;
            this.StepId = stepId;
            this.Action = action;
        }

        public long TutorialId { get; set; }

        public long StepId { get; set; }

        public TutorialAction Action { get; set; }
    }
}

