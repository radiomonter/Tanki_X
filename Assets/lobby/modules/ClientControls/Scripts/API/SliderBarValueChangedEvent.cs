namespace Assets.lobby.modules.ClientControls.Scripts.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SliderBarValueChangedEvent : Event
    {
        public SliderBarValueChangedEvent()
        {
        }

        public SliderBarValueChangedEvent(float value)
        {
            this.Value = value;
        }

        public float Value { get; set; }
    }
}

