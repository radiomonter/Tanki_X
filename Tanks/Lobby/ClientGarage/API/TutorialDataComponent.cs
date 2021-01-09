namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TutorialDataComponent : Component
    {
        public string StepsPath { get; set; }

        public long TutorialId { get; set; }

        public Dictionary<string, long> Steps { get; set; }

        [ProtocolOptional]
        public bool ForNewPlayer { get; set; }

        [ProtocolOptional]
        public bool ForOldPlayer { get; set; }
    }
}

