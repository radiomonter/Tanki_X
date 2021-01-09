namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    [SerialVersionUID(0x15e32a3f52dL)]
    public class DisbalanceInfoComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private Tanks.Battle.ClientHUD.Impl.Timer timer;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private TextMeshProUGUI tmp;
        [SerializeField]
        private LocalizedField winCtfUid;
        [SerializeField]
        private LocalizedField looseCtfUid;
        [SerializeField]
        private LocalizedField winTdmUid;
        [SerializeField]
        private LocalizedField looseTdmUid;

        public void HideDisbalanceInfo()
        {
            this.animator.SetTrigger("Hide");
        }

        private void OnDisable()
        {
            this.animator.SetTrigger("Enable");
        }

        public void ShowDisbalanceInfo(bool winner, BattleMode battleMode)
        {
            string str = "www";
            if (battleMode == BattleMode.CTF)
            {
                str = !winner ? this.looseCtfUid.Value : this.winCtfUid.Value;
            }
            else if (battleMode == BattleMode.TDM)
            {
                str = !winner ? this.looseTdmUid.Value : this.winTdmUid.Value;
            }
            this.tmp.text = str;
            this.animator.SetTrigger("Show");
            base.SendMessage("RefreshCurve");
        }

        public Tanks.Battle.ClientHUD.Impl.Timer Timer =>
            this.timer;
    }
}

