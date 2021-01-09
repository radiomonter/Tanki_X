namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e7a14ccc2L)]
    public class TutorialCompleteIdsComponent : Component
    {
        public List<long> CompletedIds { get; set; }

        public bool TutorialSkipped { get; set; }
    }
}

