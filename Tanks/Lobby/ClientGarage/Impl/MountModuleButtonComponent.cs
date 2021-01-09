namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class MountModuleButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private LocalizedField mountButtonLocalizedField;
        [SerializeField]
        private LocalizedField unmountButtonLocalizedField;
        public bool mount = true;

        public void SetEquipButtonState(int selectedSlot, bool selectedModuleMounted)
        {
            string str = !selectedModuleMounted ? this.mountButtonLocalizedField.Value : this.unmountButtonLocalizedField.Value;
            this.mount = !selectedModuleMounted;
            base.GetComponentInChildren<TextMeshProUGUI>().text = str.Replace("{0}", (selectedSlot + 1).ToString());
        }

        public bool mountButtonActive
        {
            set
            {
                base.GetComponent<Button>().interactable = value;
                base.GetComponent<CanvasGroup>().alpha = !value ? 0.2f : 1f;
            }
        }
    }
}

