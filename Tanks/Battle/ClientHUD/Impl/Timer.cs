namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(TextMeshProUGUI)), RequireComponent(typeof(UnityEngine.Animator))]
    public class Timer : MonoBehaviour
    {
        public static float battleTime;
        private bool firstUpdateTime = true;
        private UnityEngine.Animator animator;
        private TextMeshProUGUI text;
        private float lastTime;
        private float intLastTime;
        private bool autoUpdate;

        private string FormatTime(int time) => 
            $"{time / 60}:{time % 60:00}";

        public void Set(float time)
        {
            if (!Mathf.Approximately(time, this.lastTime))
            {
                this.Animator.SetFloat("Speed", (time >= 10f) ? ((float) 0) : ((float) 1));
                this.Animator.SetBool("Grow", time < 60f);
                int num = (int) time;
                if (this.firstUpdateTime)
                {
                    this.UpdateTextTime(num);
                }
                else if (num != this.intLastTime)
                {
                    this.UpdateTextTime(num);
                }
                this.lastTime = time;
                this.intLastTime = num;
                battleTime = this.intLastTime;
            }
        }

        public void Set(float time, bool autoUpdate)
        {
            this.autoUpdate = autoUpdate;
            this.Set(time);
        }

        private void Update()
        {
            if (this.autoUpdate && (this.lastTime > 0f))
            {
                this.Set(this.lastTime - Time.deltaTime);
            }
        }

        private void UpdateTextTime(int time)
        {
            this.firstUpdateTime = false;
            this.Text.text = this.FormatTime(time);
        }

        private UnityEngine.Animator Animator
        {
            get
            {
                if (this.animator == null)
                {
                    this.animator = base.GetComponent<UnityEngine.Animator>();
                }
                return this.animator;
            }
        }

        private TextMeshProUGUI Text
        {
            get
            {
                if (this.text == null)
                {
                    this.text = base.GetComponent<TextMeshProUGUI>();
                }
                return this.text;
            }
        }
    }
}

