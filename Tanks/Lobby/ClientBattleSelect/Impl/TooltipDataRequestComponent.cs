namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class TooltipDataRequestComponent : Component
    {
        public Vector3 MousePosition { get; set; }

        public GameObject TooltipPrefab { get; set; }

        public Tanks.Lobby.ClientUserProfile.API.InteractionSource InteractionSource { get; set; }

        public long idOfRequestedUser { get; internal set; }

        public long InteractableSourceId { get; set; }
    }
}

