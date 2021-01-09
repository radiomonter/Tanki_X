namespace Tanks.Lobby.ClientControls.API
{
    using System;

    public class ProgressBarUpgradeAnimation
    {
        public float progress;
        public float additionalProgress;
        public float additionalProgress1;

        public ProgressBarUpgradeAnimation(float progress, float additionalProgress, float additionalProgress1)
        {
            this.progress = progress;
            this.additionalProgress = additionalProgress;
            this.additionalProgress1 = additionalProgress1;
        }
    }
}

