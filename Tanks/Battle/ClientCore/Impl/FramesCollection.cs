namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class FramesCollection
    {
        private readonly int maxFrameDurationInMs;
        private readonly int measuringIntervalInSec;
        private StatisticCollection frames;
        private float intervalStartTime = float.NaN;
        private StatisticCollection intervalFrames;
        private int hugeFrameCount;
        private int minAverageForInterval = 0x7fffffff;
        private int maxAverageForInterval = -2147483648;

        public FramesCollection(int maxFrameDurationInMs, int measuringIntervalInSec)
        {
            this.maxFrameDurationInMs = maxFrameDurationInMs;
            this.measuringIntervalInSec = measuringIntervalInSec;
            this.frames = new StatisticCollection(maxFrameDurationInMs);
            this.intervalFrames = new StatisticCollection(maxFrameDurationInMs);
        }

        public void AddFrame(int durationInMs)
        {
            if (this.FrameIsHuge(durationInMs))
            {
                this.hugeFrameCount++;
            }
            else
            {
                this.frames.Add(durationInMs);
                if (this.CurrentIntervalNotExist())
                {
                    this.StartNewInterval();
                }
                if (this.CurrentIntervalCompleted())
                {
                    this.ProcessCurrentInterval();
                    this.StartNewInterval();
                }
                this.AddFrameToInterval(durationInMs);
            }
        }

        private void AddFrameToInterval(int durationInMs)
        {
            this.intervalFrames.Add(durationInMs);
        }

        private bool CurrentIntervalCompleted() => 
            (UnityTime.realtimeSinceStartup - this.intervalStartTime) >= this.measuringIntervalInSec;

        private bool CurrentIntervalNotExist() => 
            float.IsNaN(this.intervalStartTime);

        private bool FrameIsHuge(int durationInMs) => 
            durationInMs >= this.maxFrameDurationInMs;

        private void ProcessCurrentInterval()
        {
            if (this.intervalFrames.TotalCount != 0)
            {
                if (this.intervalFrames.Average < this.minAverageForInterval)
                {
                    this.minAverageForInterval = (int) this.intervalFrames.Average;
                }
                if (this.intervalFrames.Average > this.maxAverageForInterval)
                {
                    this.maxAverageForInterval = (int) this.intervalFrames.Average;
                }
            }
        }

        private void StartNewInterval()
        {
            this.intervalStartTime = UnityTime.realtimeSinceStartup;
            this.intervalFrames = new StatisticCollection(this.maxFrameDurationInMs);
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public int Moda =>
            this.frames.Moda;

        public int Average =>
            (int) Mathf.Round(this.frames.Average);

        public int StandartDevation =>
            (int) this.frames.StandartDeviation;

        public int HugeFrameCount =>
            this.hugeFrameCount;

        public int MinAverageForInterval
        {
            get
            {
                this.ProcessCurrentInterval();
                return this.minAverageForInterval;
            }
        }

        public int MaxAverageForInterval
        {
            get
            {
                this.ProcessCurrentInterval();
                return this.maxAverageForInterval;
            }
        }
    }
}

