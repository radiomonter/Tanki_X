namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class QuestConditionDescriptionComponent : Component
    {
        public ConditionDescription condition { get; set; }

        public string restrictionFormat { get; set; }

        public Dictionary<QuestConditionType, string> restrictions { get; set; }
    }
}

