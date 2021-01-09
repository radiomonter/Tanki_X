namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(NormalizedAnimatedValue))]
    public class AnimatedMoneyIndicator : AnimatedIndicatorWithFinishComponent<PersonalBattleResultMoneyIndicatorFinishedComponent>
    {
        [SerializeField]
        private UserMoneyIndicatorComponent indicator;
        [SerializeField]
        private Text deltaValue;
        private NormalizedAnimatedValue animation;

        private void Awake()
        {
            this.animation = base.GetComponent<NormalizedAnimatedValue>();
        }

        public void Init(Entity screenEntity, long money)
        {
            base.SetEntity(screenEntity);
            this.Money = money;
            this.deltaValue.text = string.Empty;
            base.GetComponent<Animator>().SetTrigger("Start");
        }

        private void Update()
        {
            if (this.Money <= 0L)
            {
                base.TryToSetAnimationFinished();
            }
            else
            {
                this.indicator.Suspend((long) (this.Money * (1f - this.animation.value)));
                this.deltaValue.text = "+" + ((long) (this.animation.value * this.Money)).ToStringSeparatedByThousands();
                base.TryToSetAnimationFinished(this.animation.value, 1f);
            }
        }

        private long Money { get; set; }
    }
}

