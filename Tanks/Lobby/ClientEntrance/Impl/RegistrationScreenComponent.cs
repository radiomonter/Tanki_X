namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using TMPro;
    using UnityEngine;

    public class RegistrationScreenComponent : BehaviourComponent, NoScaleScreen
    {
        [SerializeField]
        private TMP_InputField uidInput;
        [SerializeField]
        private TMP_InputField passwordInput;
        [SerializeField]
        private TMP_InputField emailInput;
        public GameObject locker;

        private void Awake()
        {
        }

        public InteractivityPrerequisiteComponent GetEmailInput() => 
            this.emailInput.GetComponent<InteractivityPrerequisiteComponent>();

        public InteractivityPrerequisiteComponent GetUidInput() => 
            this.uidInput.GetComponent<InteractivityPrerequisiteComponent>();

        public void LockScreen(bool value)
        {
            this.locker.SetActive(value);
        }

        private void OnEnable()
        {
            this.LockScreen(false);
        }

        public void SetEmailInputInteractable(bool interactable)
        {
            this.emailInput.interactable = interactable;
            if (interactable)
            {
                this.emailInput.GetComponent<Animator>().SetTrigger("Reset");
            }
            else
            {
                this.emailInput.GetComponent<Animator>().SetTrigger("Inactive");
            }
        }

        public void SetUidInputInteractable(bool interactable)
        {
            this.uidInput.interactable = interactable;
            if (interactable)
            {
                this.uidInput.GetComponent<Animator>().SetTrigger("Reset");
            }
            else
            {
                this.uidInput.GetComponent<Animator>().SetTrigger("Inactive");
            }
        }

        public virtual string Uid
        {
            get => 
                this.uidInput.text;
            set => 
                this.uidInput.text = value;
        }

        public virtual string Password =>
            this.passwordInput.text;

        public virtual string Email
        {
            get => 
                this.emailInput.text;
            set => 
                this.emailInput.text = value;
        }
    }
}

