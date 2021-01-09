namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    [RequireComponent(typeof(InputFieldComponent))]
    public class EmailInputFieldComponent : LocalizedControl, Component
    {
        [SerializeField, Tooltip("Если true - переводит инпут в валидное состояние если email существует, не валидное - если не существует")]
        private bool existsIsValid;
        [SerializeField, Tooltip("Если true - дополнительно проверяет в неподтверждённых")]
        private bool includeUnconfirmed;

        public bool ExistsIsValid =>
            this.existsIsValid;

        public bool IncludeUnconfirmed =>
            this.includeUnconfirmed;

        public string Hint
        {
            set => 
                base.GetComponent<InputFieldComponent>().Hint = value;
        }

        public string EmailIsInvalid { get; set; }

        public string EmailIsOccupied { get; set; }

        public string EmailIsNotConfirmed { get; set; }
    }
}

