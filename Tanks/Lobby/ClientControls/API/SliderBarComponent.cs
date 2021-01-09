namespace Tanks.Lobby.ClientControls.API
{
    using Assets.lobby.modules.ClientControls.Scripts.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [ExecuteInEditMode, RequireComponent(typeof(Slider))]
    public class SliderBarComponent : BehaviourComponent, AttachToEntityListener
    {
        private Slider slider;

        public void AttachedToEntity(Entity entity)
        {
            <AttachedToEntity>c__AnonStorey0 storey = new <AttachedToEntity>c__AnonStorey0 {
                entity = entity,
                $this = this
            };
            this.slider = base.GetComponent<Slider>();
            this.slider.onValueChanged.AddListener(new UnityAction<float>(storey.<>m__0));
        }

        public void OnDestroy()
        {
            this.slider.onValueChanged.RemoveAllListeners();
        }

        public float Value
        {
            get => 
                this.slider.value;
            set => 
                this.slider.value = value;
        }

        [CompilerGenerated]
        private sealed class <AttachedToEntity>c__AnonStorey0
        {
            internal Entity entity;
            internal SliderBarComponent $this;

            internal void <>m__0(float value)
            {
                if (this.$this.slider.minValue.Equals(value))
                {
                    ECSBehaviour.EngineService.Engine.ScheduleEvent(new SliderBarSetToMinValueEvent(), this.entity);
                }
                else
                {
                    ECSBehaviour.EngineService.Engine.ScheduleEvent(new SliderBarValueChangedEvent(value), this.entity);
                }
            }
        }
    }
}

