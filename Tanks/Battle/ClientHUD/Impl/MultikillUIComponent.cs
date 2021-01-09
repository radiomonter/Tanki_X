namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class MultikillUIComponent : BehaviourComponent
    {
        private static string ACTIVATE_TRIGGER = "Activate";
        private static string DEACTIVATE_TRIGGER = "Deactivate";
        [SerializeField]
        private UnityEngine.Animator animator;
        [SerializeField]
        private AssetReference voiceReference;
        [SerializeField]
        private LocalizedField multikillText;
        [SerializeField]
        private LocalizedField streakText;
        [SerializeField]
        private TextMeshProUGUI multikillTextField;
        [SerializeField]
        private TextMeshProUGUI streakTextField;
        [SerializeField]
        private AnimatedLong scoreText;
        [SerializeField]
        private bool disableVoice;
        private Coroutine coroutine;
        private GameObject voiceInstance;

        public void ActivateEffect(int score = 0, int kills = 0, string userName = "")
        {
            if ((this.multikillText != null) && !string.IsNullOrEmpty(this.multikillText.Value))
            {
                this.multikillTextField.text = this.multikillText.Value;
            }
            this.scoreText.Value = score;
            if (kills > 0)
            {
                this.streakTextField.text = string.Format(this.streakText.Value, kills);
            }
            else if (string.IsNullOrEmpty(userName))
            {
                this.streakTextField.gameObject.SetActive(false);
            }
            else
            {
                this.streakTextField.text = string.Format(this.streakText.Value, userName);
                this.streakTextField.gameObject.SetActive(true);
            }
            this.CancelCoroutine();
            this.coroutine = base.StartCoroutine(this.SetTrigger(ACTIVATE_TRIGGER));
        }

        private void CancelCoroutine()
        {
            if (this.coroutine != null)
            {
                base.StopCoroutine(this.coroutine);
                this.coroutine = null;
            }
        }

        public void DeactivateEffect()
        {
            this.CancelCoroutine();
            this.animator.SetTrigger(DEACTIVATE_TRIGGER);
        }

        public void PlayVoice()
        {
            if ((this.Voice != null) && !this.disableVoice)
            {
                this.voiceInstance = Instantiate<GameObject>(this.Voice);
            }
        }

        [DebuggerHidden]
        private IEnumerator SetTrigger(string state) => 
            new <SetTrigger>c__Iterator0 { 
                state = state,
                $this = this
            };

        public void StopVoice()
        {
            if (this.Voice != null)
            {
                Destroy(this.voiceInstance);
            }
        }

        public UnityEngine.Animator Animator =>
            this.animator;

        public AssetReference VoiceReference =>
            this.voiceReference;

        public GameObject Voice { get; set; }

        [CompilerGenerated]
        private sealed class <SetTrigger>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal string state;
            internal MultikillUIComponent $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = new WaitForEndOfFrame();
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.animator.SetTrigger(this.state);
                        this.$PC = -1;
                        break;

                    default:
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

