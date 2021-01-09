namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class LeaguePlaceUIComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI placeText;
        [SerializeField]
        private LocalizedField placeLocalizedField;
        [SerializeField]
        private List<GameObject> elements;

        public void Hide()
        {
            this.SetElementsVisibility(false);
        }

        private void SetElementsVisibility(bool visibility)
        {
            <SetElementsVisibility>c__AnonStorey0 storey = new <SetElementsVisibility>c__AnonStorey0 {
                visibility = visibility
            };
            this.elements.ForEach(new Action<GameObject>(storey.<>m__0));
        }

        public void SetPlace(int place)
        {
            this.placeText.text = this.placeLocalizedField.Value + "\n" + place;
            this.Show();
        }

        private void Show()
        {
            this.SetElementsVisibility(true);
        }

        [CompilerGenerated]
        private sealed class <SetElementsVisibility>c__AnonStorey0
        {
            internal bool visibility;

            internal void <>m__0(GameObject element)
            {
                element.SetActive(this.visibility);
            }
        }
    }
}

