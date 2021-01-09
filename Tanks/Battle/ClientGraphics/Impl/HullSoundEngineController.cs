namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Linq;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class HullSoundEngineController : MonoBehaviour
    {
        private const string EMPTY_RPM_DATA_EXCEPTION = "No data for hull sound engine";
        private const float DEFAULT_ENGINE_LOAD = 0.5f;
        private const float HESITATION_LEVEL_MIN = -1f;
        private const float HESITATION_LEVEL_MAX = 1f;
        private const float EPS = 0.001f;
        [SerializeField]
        private RPMSoundBehaviour[] RPMSoundBehaviourArray;
        [SerializeField]
        private bool enableAudioSourceOptimizing = true;
        [SerializeField]
        private bool useAudioFilters = true;
        [SerializeField, Range(0f, 1f)]
        private float blendRange = 0.9f;
        [SerializeField]
        private float extremalRPMStartOffset = 2f;
        [SerializeField]
        private float extremalRPMEndOffset = 100f;
        [SerializeField, HideInInspector]
        private float minRPM;
        [SerializeField, HideInInspector]
        private float maxRPM;
        [SerializeField, HideInInspector]
        private int RPMDataArrayLength;
        [SerializeField, HideInInspector]
        private int lastRPMDataIndex;
        [SerializeField]
        private float acelerationRPMFactor = 1.5f;
        [SerializeField]
        private float decelerationRPMFactor = 1.5f;
        [SerializeField, Range(0f, 1f)]
        private float increasingLoadThreshold = 0.02857143f;
        [SerializeField, Range(0f, 1f)]
        private float decreasingLoadThreshold = 0.02857143f;
        [SerializeField]
        private float increasingLoadSpeed = 0.05f;
        [SerializeField]
        private float decreasingLoadSpeed = 0.05f;
        [SerializeField]
        private float increasingRPMSpeed = 10f;
        [SerializeField]
        private float decreasingRPMSpeed = 10f;
        [SerializeField]
        private float hesitationAmplitudeRPM = 30f;
        [SerializeField]
        private float hesitationAmplitudeLoad = 0.2f;
        [SerializeField]
        private float hesitationFrequency = 1f;
        [SerializeField]
        private float hesitationShockMinInterval = 0.5f;
        [SerializeField]
        private float hesitationShockMaxInterval = 2f;
        [SerializeField]
        private float fadeInTimeSec = 2f;
        [SerializeField]
        private float fadeOutTimeSec = 2f;
        [SerializeField, Range(0f, 1f)]
        private float inputRPMFactor;
        private bool selfEngine;
        private int startSoundIndex;
        private int endSoundIndex;
        private float engineLoad;
        private float previousEngineRPM;
        private float engineRPM;
        private float targetEngineRPM;
        private float engineVolume;
        private float engineVolumeSpeed;
        private float fadeInSpeed;
        private float realRPMFadeOutSpeed;
        private float destroyTimer;
        private float hesitationLevel;
        private float hesitationShockIntervalTimer;
        private float timerOfHesitationsInStableWork;
        private bool isStableWork;
        private float realRPMFactor;

        private void Awake()
        {
            base.enabled = false;
        }

        public void Build()
        {
            this.RPMDataArrayLength = this.RPMSoundBehaviourArray.Length;
            if (this.RPMDataArrayLength == 0)
            {
                base.enabled = false;
                throw new Exception("No data for hull sound engine");
            }
            this.RPMSoundBehaviourArray = this.RPMSoundBehaviourArray.ToList<RPMSoundBehaviour>().OrderBy<RPMSoundBehaviour, float>(new Func<RPMSoundBehaviour, float>(this.SortByRPMProperty)).ToArray<RPMSoundBehaviour>();
            this.lastRPMDataIndex = this.RPMDataArrayLength - 1;
            RPMSoundBehaviour behaviour = this.RPMSoundBehaviourArray[0];
            RPMSoundBehaviour behaviour2 = this.RPMSoundBehaviourArray[this.lastRPMDataIndex];
            this.minRPM = behaviour.RPM;
            this.maxRPM = behaviour2.RPM;
            for (int i = 0; i < this.RPMDataArrayLength; i++)
            {
                float prevRPM = (i != 0) ? this.RPMSoundBehaviourArray[i - 1].RPM : (this.minRPM - ((this.extremalRPMStartOffset + this.hesitationAmplitudeRPM) / this.blendRange));
                this.RPMSoundBehaviourArray[i].Build(this, prevRPM, (i != this.lastRPMDataIndex) ? this.RPMSoundBehaviourArray[i + 1].RPM : (this.maxRPM + ((this.extremalRPMEndOffset + this.hesitationAmplitudeRPM) / this.blendRange)), this.blendRange);
            }
        }

        private void ClampEngineVolume()
        {
            if (this.engineVolume > 1f)
            {
                this.engineVolume = 1f;
                this.engineVolumeSpeed = 0f;
            }
            if (this.engineVolume < 0f)
            {
                this.engineVolume = 0f;
                this.engineVolumeSpeed = 0f;
            }
        }

        private float GetHesitationShockInterval() => 
            Random.Range(this.hesitationShockMinInterval, this.hesitationShockMaxInterval);

        private void IncrementEngineVolume(float dt)
        {
            if (this.engineVolumeSpeed != 0f)
            {
                this.engineVolume += this.engineVolumeSpeed * dt;
            }
        }

        public void Init(bool self)
        {
            this.RealRPMFactor = this.InputRpmFactor;
            this.fadeInSpeed = 1f / this.fadeInTimeSec;
            this.engineVolumeSpeed = 0f;
            this.selfEngine = self;
            base.gameObject.SetActive(true);
        }

        private bool IsDestroyed(float dt)
        {
            if (this.destroyTimer > 0f)
            {
                this.destroyTimer -= dt;
                if (this.destroyTimer <= 0f)
                {
                    this.StopEngineSounds();
                    return true;
                }
            }
            return false;
        }

        private bool IsRPMAboveBeginRange(float rpm, RPMSoundBehaviour rpmSoundBehaviour) => 
            rpm >= rpmSoundBehaviour.RangeBeginRpm;

        private bool IsRPMBelowEndRange(float rpm, RPMSoundBehaviour rpmSoundBehaviour) => 
            rpm < rpmSoundBehaviour.RangeEndRpm;

        public bool IsRPMWithinRange(RPMSoundBehaviour rpmSoundBehaviour, float rpm) => 
            this.IsRPMAboveBeginRange(rpm, rpmSoundBehaviour) && this.IsRPMBelowEndRange(rpm, rpmSoundBehaviour);

        public void Play()
        {
            this.engineVolume = 0f;
            this.engineVolumeSpeed = this.fadeInSpeed;
            this.engineLoad = 0.5f;
            this.InputRpmFactor = 0f;
            this.RealRPMFactor = this.InputRpmFactor;
            this.engineRPM = this.minRPM;
            this.previousEngineRPM = this.EngineRpm;
            this.realRPMFadeOutSpeed = 0f;
            this.destroyTimer = -1f;
            if (this.enableAudioSourceOptimizing)
            {
                this.PlayAppropriateEngineSounds();
            }
            else
            {
                this.PlayAllEngineSounds();
            }
            base.enabled = true;
        }

        private void PlayAllEngineSounds()
        {
            this.startSoundIndex = 0;
            this.endSoundIndex = this.lastRPMDataIndex;
            for (int i = this.startSoundIndex; i <= this.endSoundIndex; i++)
            {
                this.RPMSoundBehaviourArray[i].Play(this.engineVolume);
            }
        }

        private void PlayAppropriateEngineSounds()
        {
            bool flag = false;
            for (int i = 0; i < this.RPMDataArrayLength; i++)
            {
                RPMSoundBehaviour rpmSoundBehaviour = this.RPMSoundBehaviourArray[i];
                if (!this.IsRPMWithinRange(rpmSoundBehaviour, this.EngineRpm))
                {
                    rpmSoundBehaviour.Stop();
                }
                else
                {
                    if (!flag)
                    {
                        flag = true;
                        this.startSoundIndex = i;
                    }
                    rpmSoundBehaviour.Play(this.engineVolume);
                    this.endSoundIndex = i;
                }
            }
        }

        private float SortByRPMProperty(RPMSoundBehaviour currentRPMSoundBehaviour) => 
            currentRPMSoundBehaviour.RPM;

        public void Stop()
        {
            this.engineVolumeSpeed = (this.engineVolume != 0f) ? (-this.engineVolume / this.fadeOutTimeSec) : -1f;
            this.realRPMFadeOutSpeed = (this.RealRPMFactor != 0f) ? (-this.RealRPMFactor / this.fadeOutTimeSec) : -1f;
            this.destroyTimer = this.fadeOutTimeSec;
        }

        private void StopEngineSounds()
        {
            for (int i = this.startSoundIndex; i <= this.endSoundIndex; i++)
            {
                this.RPMSoundBehaviourArray[i].Stop();
            }
            base.enabled = false;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (!this.IsDestroyed(deltaTime))
            {
                this.IncrementEngineVolume(deltaTime);
                this.UpdateRealRPMFactor(deltaTime);
                this.UpdateEngineRPM(deltaTime);
                this.UpdateEngineLoad(deltaTime);
                this.UpdateEngineSounds();
            }
        }

        private void UpdateCurrentEngineSoundList()
        {
            int startSoundIndex = this.startSoundIndex;
            int endSoundIndex = this.endSoundIndex;
            bool flag = true;
            for (int i = this.startSoundIndex; i <= this.endSoundIndex; i++)
            {
                RPMSoundBehaviour behaviour = this.RPMSoundBehaviourArray[i];
                if (behaviour.NeedToStop)
                {
                    behaviour.Stop();
                }
                else
                {
                    behaviour.Play(this.engineVolume);
                    endSoundIndex = i;
                    if (flag)
                    {
                        flag = false;
                        startSoundIndex = i;
                    }
                }
            }
            this.endSoundIndex = endSoundIndex;
            this.startSoundIndex = startSoundIndex;
        }

        private void UpdateEngineLoad(float dt)
        {
            float engineLoad = this.engineLoad;
            if (this.IsStableWork)
            {
                engineLoad = 0.5f + (this.hesitationAmplitudeLoad * this.hesitationLevel);
            }
            else
            {
                engineLoad = this.inputRPMFactor - this.RealRPMFactor;
                float num3 = Mathf.Abs(engineLoad);
                float num4 = Mathf.Sign(engineLoad);
                float num5 = (engineLoad < 0f) ? this.decreasingLoadThreshold : this.increasingLoadThreshold;
                engineLoad = (num3 <= num5) ? (engineLoad / num5) : num4;
                engineLoad = Mathf.Clamp01((engineLoad + 1f) * 0.5f);
            }
            if (engineLoad != this.engineLoad)
            {
                float num6 = this.engineLoad;
                num6 = ((engineLoad - this.engineLoad) <= 0f) ? Mathf.Max(num6 - (this.decreasingLoadSpeed * dt), engineLoad) : Mathf.Min(num6 + (this.increasingLoadSpeed * dt), engineLoad);
                this.engineLoad = Mathf.Clamp01(num6);
            }
        }

        private void UpdateEngineRPM(float dt)
        {
            this.previousEngineRPM = this.engineRPM;
            float engineRPM = this.engineRPM;
            engineRPM = ((this.targetEngineRPM - this.engineRPM) <= 0f) ? Mathf.Max(engineRPM - (this.decreasingRPMSpeed * dt), this.targetEngineRPM) : Mathf.Min(engineRPM + (this.increasingRPMSpeed * dt), this.targetEngineRPM);
            this.engineRPM = Mathf.Clamp(engineRPM, this.minRPM, this.maxRPM);
        }

        private void UpdateEngineSounds()
        {
            float num = this.EngineRpm - this.previousEngineRPM;
            if (!this.enableAudioSourceOptimizing)
            {
                this.UpdateEngineSoundsVolume();
            }
            else
            {
                if (num == 0f)
                {
                    this.UpdateCurrentEngineSoundList();
                    return;
                }
                if (num > 0f)
                {
                    this.UpdateEngineSoundsStraight();
                }
                else
                {
                    this.UpdateEngineSoundsReverse();
                }
            }
            this.ClampEngineVolume();
        }

        private void UpdateEngineSoundsReverse()
        {
            int endSoundIndex = this.endSoundIndex;
            int num2 = -1;
            int num3 = -1;
            while (true)
            {
                if (endSoundIndex != num2)
                {
                    RPMSoundBehaviour rpmSoundBehaviour = this.RPMSoundBehaviourArray[endSoundIndex];
                    if (!this.IsRPMAboveBeginRange(this.EngineRpm, rpmSoundBehaviour) && rpmSoundBehaviour.NeedToStop)
                    {
                        rpmSoundBehaviour.Stop();
                        this.endSoundIndex += num3;
                        endSoundIndex += num3;
                        continue;
                    }
                    if (this.IsRPMBelowEndRange(this.EngineRpm, rpmSoundBehaviour))
                    {
                        rpmSoundBehaviour.Play(this.engineVolume);
                        this.startSoundIndex = endSoundIndex;
                        endSoundIndex += num3;
                        continue;
                    }
                    rpmSoundBehaviour.Stop();
                }
                return;
            }
        }

        private void UpdateEngineSoundsStraight()
        {
            int startSoundIndex = this.startSoundIndex;
            int rPMDataArrayLength = this.RPMDataArrayLength;
            int num3 = 1;
            while (true)
            {
                if (startSoundIndex != rPMDataArrayLength)
                {
                    RPMSoundBehaviour rpmSoundBehaviour = this.RPMSoundBehaviourArray[startSoundIndex];
                    if (!this.IsRPMBelowEndRange(this.EngineRpm, rpmSoundBehaviour) && rpmSoundBehaviour.NeedToStop)
                    {
                        rpmSoundBehaviour.Stop();
                        this.startSoundIndex += num3;
                        startSoundIndex += num3;
                        continue;
                    }
                    if (this.IsRPMAboveBeginRange(this.EngineRpm, rpmSoundBehaviour))
                    {
                        rpmSoundBehaviour.Play(this.engineVolume);
                        this.endSoundIndex = startSoundIndex;
                        startSoundIndex += num3;
                        continue;
                    }
                    rpmSoundBehaviour.Stop();
                }
                return;
            }
        }

        private void UpdateEngineSoundsVolume()
        {
            if (this.engineVolumeSpeed != 0f)
            {
                for (int i = this.startSoundIndex; i <= this.endSoundIndex; i++)
                {
                    this.RPMSoundBehaviourArray[i].Play(this.engineVolume);
                }
            }
        }

        private void UpdateRealRPMFactor(float dt)
        {
            float a = this.inputRPMFactor - this.RealRPMFactor;
            this.IsStableWork = MathUtil.NearlyEqual(a, 0f, 0.001f);
            if (this.destroyTimer > 0f)
            {
                this.RealRPMFactor += this.realRPMFadeOutSpeed * dt;
            }
            else if (!this.IsStableWork)
            {
                this.RealRPMFactor += (a * ((a <= 0f) ? this.decelerationRPMFactor : this.acelerationRPMFactor)) * dt;
            }
            else
            {
                this.RealRPMFactor = this.inputRPMFactor;
                this.timerOfHesitationsInStableWork += dt;
                this.hesitationShockIntervalTimer -= dt;
            }
        }

        public bool SelfEngine =>
            this.selfEngine;

        private bool IsStableWork
        {
            get => 
                this.isStableWork;
            set
            {
                if (this.isStableWork != value)
                {
                    if (value)
                    {
                        this.timerOfHesitationsInStableWork = 0f;
                        this.hesitationShockIntervalTimer = this.GetHesitationShockInterval();
                    }
                    this.isStableWork = value;
                }
            }
        }

        private float RealRPMFactor
        {
            get => 
                this.realRPMFactor;
            set
            {
                this.realRPMFactor = (this.realRPMFactor >= this.inputRPMFactor) ? Mathf.Max(value, this.inputRPMFactor) : Mathf.Min(value, this.inputRPMFactor);
                this.targetEngineRPM = Mathf.Lerp(this.minRPM, this.maxRPM, this.realRPMFactor);
                if (!this.IsStableWork)
                {
                    this.hesitationLevel = 0f;
                }
                else if (this.hesitationShockIntervalTimer > 0f)
                {
                    this.hesitationLevel = Mathf.Sin(this.hesitationFrequency * this.timerOfHesitationsInStableWork);
                }
                else
                {
                    this.hesitationLevel = (this.hesitationLevel < 0f) ? Random.Range((float) 0.5f, (float) 1f) : Random.Range((float) -1f, (float) -0.5f);
                    this.timerOfHesitationsInStableWork = Mathf.Asin(this.hesitationLevel) / this.hesitationFrequency;
                    this.hesitationShockIntervalTimer = this.GetHesitationShockInterval();
                }
                float num = this.hesitationAmplitudeRPM * this.hesitationLevel;
                this.targetEngineRPM += num;
            }
        }

        public float InputRpmFactor
        {
            get => 
                this.inputRPMFactor;
            set => 
                this.inputRPMFactor = value;
        }

        public float EngineRpm =>
            this.engineRPM;

        public float EngineLoad =>
            this.engineLoad;

        public bool UseAudioFilters =>
            this.useAudioFilters;
    }
}

