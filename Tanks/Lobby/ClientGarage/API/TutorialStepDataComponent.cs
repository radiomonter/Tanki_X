namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TutorialStepDataComponent : Component
    {
        public long StepId { get; set; }

        public string Message { get; set; }

        public string StepDesc { get; set; }

        public bool CanNotSkip { get; set; }

        [ProtocolOptional]
        public bool VisualStep { get; set; }
    }
}

