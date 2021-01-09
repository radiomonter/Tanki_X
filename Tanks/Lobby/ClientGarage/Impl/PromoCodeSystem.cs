namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class PromoCodeSystem : ECSSystem
    {
        private static string LINK_PREFIX = "link:";

        [OnEventFire]
        public void ActivatePromoCode(ButtonClickEvent e, ActivateButtonNode button, [JoinByScreen] InputFieldNode inputField, [JoinAll] SelfUserNode selfUser)
        {
            string str = inputField.inputField.Input.Trim();
            if (str.StartsWith(LINK_PREFIX))
            {
                base.Log.InfoFormat("NavigateLink {0}", str);
                NavigateLinkEvent eventInstance = new NavigateLinkEvent {
                    Link = str.Substring(LINK_PREFIX.Length)
                };
                base.ScheduleEvent(eventInstance, button);
            }
            else
            {
                base.Log.InfoFormat("ActivatePromoCode {0}", str);
                ActivatePromoCodeEvent eventInstance = new ActivatePromoCodeEvent {
                    Code = str
                };
                base.ScheduleEvent(eventInstance, selfUser);
                button.buttonMapping.Button.interactable = false;
                inputField.inputField.Input = string.Empty;
            }
        }

        [OnEventFire]
        public void InputChanged(InputFieldValueChangedEvent e, InputFieldNode inputField, [JoinAll] SelfUserNode selfUser)
        {
            inputField.esm.Esm.ChangeState<InputFieldStates.AwaitState>();
            inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            base.Log.InfoFormat("InputChanged", new object[0]);
        }

        [OnEventFire]
        public void InputChangedWithDelay(InputPausedEvent e, InputFieldNode inputField, [JoinAll] SelfUserNode selfUser)
        {
            string str = inputField.inputField.Input.Trim();
            if (!string.IsNullOrEmpty(str))
            {
                inputField.esm.Esm.ChangeState<InputFieldStates.AwaitState>();
                inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
                base.Log.InfoFormat("InputChangedWithDelay {0}", str);
                CheckPromoCodeEvent eventInstance = new CheckPromoCodeEvent {
                    Code = str
                };
                base.ScheduleEvent(eventInstance, selfUser);
            }
        }

        [OnEventFire]
        public void InputDisabled(NodeRemoveEvent e, InputFieldNode inputNode)
        {
            inputNode.inputField.Input = string.Empty;
        }

        [OnEventFire]
        public void ShowCheckResult(PromoCodeCheckResultEvent e, SelfUserNode selfUser, [JoinAll] InputFieldNode inputField, [JoinAll] SingleNode<PromoCodesScreenLocalizationComponent> promoCodesScreen)
        {
            string str = inputField.inputField.Input.Trim();
            base.Log.InfoFormat("ShowCheckResult {0}", e.Result);
            if (str.StartsWith(LINK_PREFIX))
            {
                base.Log.InfoFormat("ShowCheckResult IsLink {0}", str);
                inputField.esm.Esm.ChangeState<InputFieldStates.ValidState>();
                inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
            }
            else
            {
                inputField.inputField.ErrorMessage = string.IsNullOrEmpty(str) ? string.Empty : promoCodesScreen.component.InputStateTexts[e.Result.ToString()].ToString();
                if (e.Code.Equals(str, StringComparison.OrdinalIgnoreCase))
                {
                    if (e.Result == PromoCodeCheckResult.VALID)
                    {
                        inputField.esm.Esm.ChangeState<InputFieldStates.ValidState>();
                        inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
                    }
                    else
                    {
                        inputField.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
                        inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
                    }
                }
            }
        }

        public class ActivateButtonNode : Node
        {
            public ActivatePromocodeButtonComponent activatePromoCodeButton;
            public ButtonMappingComponent buttonMapping;
        }

        public class InputFieldNode : Node
        {
            public InteractivityPrerequisiteComponent interactivityPrerequisite;
            public PromoCodeInputFieldComponent promoCodeInputField;
            public InputFieldComponent inputField;
            public ESMComponent esm;
            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
        }
    }
}

