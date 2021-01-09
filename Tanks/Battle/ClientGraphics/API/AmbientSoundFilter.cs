namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Runtime.CompilerServices;

    public class AmbientSoundFilter : SingleFadeSoundFilter
    {
        private volatile float filterVolume;

        protected override void Awake()
        {
            base.Awake();
            this.FilterVolume = 0f;
        }

        protected override float FilterVolume
        {
            get => 
                this.filterVolume;
            set => 
                this.filterVolume = value;
        }
    }
}

