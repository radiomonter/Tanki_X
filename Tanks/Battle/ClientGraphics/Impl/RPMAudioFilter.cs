namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class RPMAudioFilter : AbstractRPMSoundUpdater
    {
        private float previousEngineRPM;
        private float previousEngineLoad;

        private unsafe void OnAudioFilterRead(float[] data, int channels)
        {
            int length = data.Length;
            bool flag = true;
            for (int i = 0; i < length; i++)
            {
                float t = ((float) i) / ((float) length);
                float rpm = Mathf.Lerp(this.previousEngineRPM, base.engine.EngineRpm, t);
                float smoothedLoad = Mathf.Lerp(this.previousEngineLoad, base.engine.EngineLoad, t);
                if (!base.engine.IsRPMWithinRange(base.rpmSoundBehaviour, rpm))
                {
                    data[i] = 0f;
                    flag &= true;
                }
                else if (!base.parentModifier.CheckLoad(smoothedLoad))
                {
                    data[i] = 0f;
                    bool flag1 = flag;
                    flag = false;
                }
                else
                {
                    float* singlePtr1 = &(data[i]);
                    singlePtr1[0] *= base.parentModifier.RpmSoundVolume;
                    float* singlePtr2 = &(data[i]);
                    singlePtr2[0] *= base.parentModifier.CalculateModifier(rpm, smoothedLoad);
                    bool flag2 = flag;
                    flag = false;
                }
            }
            base.parentModifier.NeedToStop = flag;
            this.UpdatePreviousParameters();
        }

        public override void Play()
        {
            if (!base.parentModifier.Source.isPlaying)
            {
                this.UpdatePreviousParameters();
            }
            base.Play();
        }

        private void UpdatePreviousParameters()
        {
            this.previousEngineLoad = base.engine.EngineLoad;
            this.previousEngineRPM = base.engine.EngineRpm;
        }
    }
}

