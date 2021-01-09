namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(InviteDialogComponent))]
    public class InviteToSquadDialogComponent : UIBehaviour, Component
    {
        public long FromUserId;
        public long SquadId;
        public long EngineId;
        public bool invite;
        [SerializeField]
        private LocalizedField inviteMessageLocalizedField;
        [SerializeField]
        private LocalizedField requestMessageLocalizedField;

        public void Hide()
        {
            base.GetComponent<InviteDialogComponent>().OnNo();
        }

        public void Show(string userUid, bool inBattle, bool invite)
        {
            this.invite = invite;
            base.GetComponent<InviteDialogComponent>().Show(!invite ? string.Format(this.requestMessageLocalizedField.Value, "<color=orange>" + userUid + "</color>", "\n") : string.Format(this.inviteMessageLocalizedField.Value, "<color=orange>" + userUid + "</color>", "\n"), inBattle);
        }
    }
}

