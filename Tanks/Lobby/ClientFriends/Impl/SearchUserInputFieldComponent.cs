namespace Tanks.Lobby.ClientFriends.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;

    public class SearchUserInputFieldComponent : BehaviourComponent
    {
        private TMP_InputField inputField;

        public void Start()
        {
            this.inputField = base.GetComponent<TMP_InputField>();
        }

        public string SearchString
        {
            get => 
                this.inputField.text;
            set => 
                this.inputField.text = value;
        }
    }
}

