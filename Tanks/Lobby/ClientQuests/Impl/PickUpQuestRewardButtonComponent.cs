namespace Tanks.Lobby.ClientQuests.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class PickUpQuestRewardButtonComponent : BehaviourComponent
    {
        public Entity QuestEntity { get; set; }
    }
}

