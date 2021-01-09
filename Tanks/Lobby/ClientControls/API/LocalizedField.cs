namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    [Serializable]
    public class LocalizedField
    {
        [SerializeField]
        private string uid;

        public static implicit operator string(LocalizedField field) => 
            field.Value;

        public string Value =>
            LocalizationUtils.Localize(this.uid).Replace(@"\n", "\n");

        public string Uid
        {
            get => 
                this.uid;
            set => 
                this.uid = value;
        }
    }
}

