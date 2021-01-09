namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button)), RequireComponent(typeof(UnityEngine.Animator))]
    public class PlayButtonComponent : EventMappingComponent
    {
        private UnityEngine.Animator animator;
        public GameObject cancelSearchButton;
        public GameObject goToLobbyButton;
        public GameObject exitLobbyButton;
        public MatchSearchingGUIComponent matchSearchingGui;
        private string lastAnimatorTrigger;
        [SerializeField]
        private LocalizedField playersInLobby;
        [SerializeField]
        private TextMeshProUGUI gameModeTitleLabel;
        [SerializeField]
        private TextMeshProUGUI playersInLobbyLabel;

        public void InitializeMatchSearchingWaitTime(bool newbieMode)
        {
            this.matchSearchingGui.SetWaitingTime(newbieMode);
        }

        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(this.lastAnimatorTrigger))
            {
                this.SetAnimatorTrigger(this.lastAnimatorTrigger);
            }
        }

        private void OnTimerExpired()
        {
            this.SendEvent<PlayButtonTimerExpiredEvent>();
        }

        public void RunTheStopwatch()
        {
            this.StopTheTimer();
            base.GetComponent<StopWatchComponent>().RunTheStopwatch();
        }

        public void RunTheTimer(Date startTime, bool matchBeginning = false)
        {
            this.StopTheStopwatch();
            base.GetComponent<PlayButtonTimerComponent>().RunTheTimer(startTime, matchBeginning);
        }

        public void SetAnimatorTrigger(string trigger)
        {
            this.lastAnimatorTrigger = trigger;
            if (this.Animator.isActiveAndEnabled)
            {
                AnimatorControllerParameter[] parameters = this.Animator.parameters;
                int index = 0;
                while (true)
                {
                    if (index >= parameters.Length)
                    {
                        this.Animator.SetBool(this.lastAnimatorTrigger, true);
                        break;
                    }
                    AnimatorControllerParameter parameter = parameters[index];
                    this.Animator.SetBool(parameter.name, false);
                    index++;
                }
            }
        }

        public void SetCustomModeTitle(string modeName, int currentPlayersCount, int maxPlayersCount)
        {
            this.gameModeTitleLabel.text = modeName;
            object[] objArray1 = new object[] { this.playersInLobby.Value, "\n", currentPlayersCount, "/", maxPlayersCount };
            this.playersInLobbyLabel.text = string.Concat(objArray1);
        }

        public void ShowCancelButton(bool show)
        {
            this.cancelSearchButton.SetActive(show);
        }

        public void ShowExitLobbyButton(bool show)
        {
            this.exitLobbyButton.SetActive(show);
        }

        public void ShowGoToLobbyButton(bool show)
        {
            this.goToLobbyButton.SetActive(show);
        }

        public void StopTheStopwatch()
        {
            base.GetComponent<StopWatchComponent>().StopTheStopwatch();
        }

        public void StopTheTimer()
        {
            base.GetComponent<PlayButtonTimerComponent>().StopTheTimer();
        }

        protected override void Subscribe()
        {
            PlayButtonTimerComponent component = base.GetComponent<PlayButtonTimerComponent>();
            component.onTimerExpired += new PlayButtonTimerComponent.TimerExpired(this.OnTimerExpired);
        }

        public UnityEngine.Animator Animator
        {
            get
            {
                UnityEngine.Animator animator = this.animator;
                if (this.animator == null)
                {
                    UnityEngine.Animator local1 = this.animator;
                    animator = this.animator = base.GetComponent<UnityEngine.Animator>();
                }
                return animator;
            }
        }

        public bool SearchingDefaultGameMode { get; set; }
    }
}

