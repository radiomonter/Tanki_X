namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(StarterPackTimerComponent))]
    public class StarterPackButtonComponent : BehaviourComponent
    {
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private ImageSkin buttonBG;
        private Entity packEntity;

        private void ClearAll()
        {
            this.text.text = string.Empty;
            base.GetComponent<StarterPackTimerComponent>().StopTimer();
        }

        public void OnClick()
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent<StarterPackSystem.ShowStarterPackScreen>(this.packEntity);
        }

        private void OnDisable()
        {
            this.PackEntity = null;
        }

        private void onTimerExpired()
        {
            base.gameObject.SetActive(false);
        }

        public void SetImage(string uid)
        {
            this.buttonBG.SpriteUid = uid;
        }

        public Entity PackEntity
        {
            get => 
                this.packEntity;
            set
            {
                this.packEntity = value;
                this.ClearAll();
                if (this.packEntity != null)
                {
                    this.text.text = this.packEntity.GetComponent<SpecialOfferContentLocalizationComponent>().Title;
                    StarterPackTimerComponent component = base.GetComponent<StarterPackTimerComponent>();
                    component.RunTimer((float) ((long) (this.packEntity.GetComponent<SpecialOfferEndTimeComponent>().EndDate - Date.Now)));
                    component.onTimerExpired = new StarterPackTimerComponent.TimerExpired(this.onTimerExpired);
                }
            }
        }
    }
}

