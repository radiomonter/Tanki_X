namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15bd37894ebL)]
    public class QuestConditionComponent : Component
    {
        public Dictionary<QuestConditionType, long> Condition { get; set; }
    }
}

