namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class CountryContent : MonoBehaviour, ListItemContent
    {
        [SerializeField]
        private Text countryName;
        [SerializeField]
        private ImageListSkin flag;
        private KeyValuePair<string, string> data;

        public void Select()
        {
            SelectCountryEvent eventInstance = new SelectCountryEvent {
                CountryCode = this.data.Key,
                CountryName = this.data.Value
            };
            EngineService.Engine.ScheduleEvent(eventInstance, EngineService.EntityStub);
        }

        public void SetDataProvider(object data)
        {
            this.data = (KeyValuePair<string, string>) data;
            this.countryName.text = this.data.Value;
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

