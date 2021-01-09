namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(InviteDialogComponent))]
    public class InviteToLobbyDialogComponent : UIBehaviour, Component
    {
        public long lobbyId;
        public long engineId;
        public LocalizedField messageForSingleUser;
        public LocalizedField messageForSquadLeader;
        public LocalizedField messageForSquadMember;
    }
}

