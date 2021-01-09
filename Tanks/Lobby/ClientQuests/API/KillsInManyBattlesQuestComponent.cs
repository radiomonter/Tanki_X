namespace Tanks.Lobby.ClientQuests.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15852db64faL)]
    public class KillsInManyBattlesQuestComponent : Component
    {
        public int Battles { get; set; }
    }
}

