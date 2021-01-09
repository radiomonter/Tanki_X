namespace Tanks.Lobby.ClientNotifications.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class AnimatedValueComponent : BehaviourComponent
    {
        public float animationTime;
        public AnimationCurve curve;
        [SerializeField]
        private long startValue;
        [SerializeField]
        private long maximum;
        [SerializeField]
        private long price;
        [SerializeField]
        private Slider upgradeSlider;
        [SerializeField]
        private TextMeshProUGUI upgradeCount;
        [SerializeField]
        private GameObject outline;
        private float time;
        private bool isOutline;
        [SerializeField]
        private bool canStart;
        private bool isStart;
        private bool canUpdate;
        private float startTime;

        private void Update()
        {
            if (this.canStart)
            {
                this.startTime = Time.time;
                this.canUpdate = true;
                this.canStart = false;
                this.isStart = true;
            }
            if (this.canUpdate && (this.price > 0L))
            {
                this.time = Time.time - this.startTime;
                float num = this.curve.Evaluate(this.time / this.animationTime) * (this.maximum - this.startValue);
                float num2 = (this.curve.Evaluate(this.time / this.animationTime) * (this.maximum - this.startValue)) * 100f;
                long num3 = this.startValue + ((long) num);
                long num4 = (this.startValue * 100) + ((long) num2);
                this.upgradeSlider.value = num4;
                object[] objArray1 = new object[] { string.Empty, num3, "/", this.price };
                this.upgradeCount.text = string.Concat(objArray1);
                if ((num3 >= this.price) && (this.outline != null))
                {
                    this.outline.GetComponent<Animator>().SetTrigger("Blink");
                }
            }
            if (this.canUpdate && ((this.startValue >= this.price) && ((this.outline != null) && (this.price > 0L))))
            {
                this.outline.GetComponent<Animator>().SetTrigger("Upgrade");
            }
            if (this.time >= this.animationTime)
            {
                this.canUpdate = false;
            }
            if (this.price == 0L)
            {
                this.upgradeCount.text = string.Empty;
            }
        }

        public long StartValue
        {
            set => 
                this.startValue = value;
        }

        public long Maximum
        {
            set => 
                this.maximum = value;
        }

        public long Price
        {
            set => 
                this.price = value;
        }
    }
}

