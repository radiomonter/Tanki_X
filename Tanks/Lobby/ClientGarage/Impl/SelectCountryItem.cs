namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    public class SelectCountryItem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI countryName;
        public CountrySelected countrySelected;
        private KeyValuePair<string, string> country;

        private void Awake()
        {
            base.GetComponent<Toggle>().onValueChanged.AddListener(new UnityAction<bool>(this.ToggleValueChanged));
        }

        public void Init(KeyValuePair<string, string> country)
        {
            this.country = country;
            this.CountryName = country.Value;
        }

        private void OnDestroy()
        {
            this.countrySelected = null;
        }

        private void ToggleValueChanged(bool value)
        {
            if (value && (this.countrySelected != null))
            {
                this.countrySelected(this.country);
            }
        }

        public string CountryName
        {
            get => 
                this.country.Value;
            set => 
                this.countryName.text = value;
        }

        public string CountryCode =>
            this.country.Key;
    }
}

