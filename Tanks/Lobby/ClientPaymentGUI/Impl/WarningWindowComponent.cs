namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using TMPro;
    using UnityEngine;

    public class WarningWindowComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI info;
        [SerializeField]
        private TextMeshProUGUI warning;
        [SerializeField]
        private LocalizedField warningText;
        [SerializeField]
        private PaletteColorField xCrystalColor;
        private Action onBack;
        private Action onForward;

        public void Cancel()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetTrigger("cancel");
            this.onBack();
        }

        public void Proceed()
        {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.GetComponent<Animator>().SetTrigger("cancel");
            this.onForward();
        }

        public void SetCompensation(long amount)
        {
            this.warning.text = string.Format(this.warningText.Value, $"<#{this.xCrystalColor.Color.ToHexString()}>{amount}<sprite=9></color>");
        }

        public void Show(Entity item, Action onBack, Action onForward)
        {
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Cancel));
            this.onBack = onBack;
            this.onForward = onForward;
            base.gameObject.SetActive(true);
            this.info.text = ShopDialogs.FormatItem(item, null);
            this.warning.text = string.Empty;
            Entity[] entities = new Entity[] { SelfUserComponent.SelfUser, item };
            ECSBehaviour.EngineService.Engine.NewEvent<CalculateCompensationRequestEvent>().AttachAll(entities).Schedule();
        }
    }
}

