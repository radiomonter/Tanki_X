namespace Tanks.Lobby.ClientLoading.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Video;

    public class IntroCinematicComponent : BehaviourComponent
    {
        private VideoPlayer player;
        private bool hintVisible;
        private GameObject videoPrefab;
        private Animator animator;
        [CompilerGenerated]
        private static VideoPlayer.EventHandler <>f__am$cache0;

        private void OnEnable()
        {
            this.player = base.GetComponentInChildren<VideoPlayer>(true);
            this.player.Prepare();
        }

        private void OnFinishPlay(VideoPlayer _)
        {
            this.animator.SetTrigger("HideVideo");
        }

        private void OnGUI()
        {
            if (((this.player != null) && this.player.isPlaying) && ((Event.current.type == EventType.KeyDown) || (Event.current.type == EventType.MouseDown)))
            {
                if (this.hintVisible && (Event.current.keyCode == KeyCode.Space))
                {
                    this.animator.SetTrigger("HideVideo");
                }
                else
                {
                    this.animator.SetTrigger("ShowHint");
                    this.hintVisible = true;
                }
            }
        }

        public void OnIntroHide()
        {
            Destroy(base.gameObject);
        }

        private void OnVideoLoaded(Object obj)
        {
            this.player.clip = (VideoClip) obj;
        }

        public void Play()
        {
            this.animator = base.GetComponent<Animator>();
            this.animator.SetTrigger("ShowVideo");
            this.player.SetTargetAudioSource(0, this.player.GetComponent<AudioSource>());
            this.player.loopPointReached += new VideoPlayer.EventHandler(this.OnFinishPlay);
            if (this.player.isPrepared)
            {
                this.player.Play();
            }
            else
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = source => source.Play();
                }
                this.player.prepareCompleted += <>f__am$cache0;
            }
        }
    }
}

