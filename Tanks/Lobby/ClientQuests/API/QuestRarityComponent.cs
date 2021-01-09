namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15c20484925L)]
    public class QuestRarityComponent : Component
    {
        public QuestRarityType RarityType { get; set; }
    }
}

