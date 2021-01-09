namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.InteropServices;
    using TMPro;
    using UnityEngine;

    public class BuyXCrystalsDialogComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI title;

        public void Hide()
        {
            base.GetComponent<Animator>().SetBool("show", false);
            base.gameObject.SetActive(false);
        }

        public void Show(bool showTitle = true)
        {
            MainScreenComponent.Instance.OverrideOnBack(new Action(this.Hide));
            this.title.gameObject.SetActive(showTitle);
            base.gameObject.SetActive(true);
            base.GetComponent<Animator>().SetBool("show", true);
        }

        private void Update()
        {
            if (InputMapping.Cancel)
            {
                this.Hide();
            }
        }
    }
}

