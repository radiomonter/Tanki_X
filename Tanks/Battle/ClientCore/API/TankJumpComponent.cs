namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x1ab6b215e41L), Shared, SkipExceptionOnAddRemove]
    public class TankJumpComponent : SharedChangeableComponent
    {
        public static float START_TIME = 0.06f;
        public static float NEAR_START_TIME = 0.2f;
        public static float JUMP_MAX_TIME = 10f;
        public static float SLOWDOWN_TIME = 3f;

        public void FinishAndSlowdown()
        {
            if (!this.Slowdown)
            {
                this.Slowdown = true;
                this.SlowdownStartTime = Time.timeSinceLevelLoad;
            }
        }

        public float GetSlowdownLerp()
        {
            if (this.isNearBegin())
            {
                return 0f;
            }
            if (!this.Slowdown)
            {
                return 0f;
            }
            float num = Mathf.Clamp((float) ((Time.timeSinceLevelLoad - this.SlowdownStartTime) / (SLOWDOWN_TIME * 0.8f)), (float) 0f, (float) 1f);
            return (num * num);
        }

        public bool isBegin() => 
            this.OnFly && ((Time.timeSinceLevelLoad - this.StartTime) < START_TIME);

        public bool isFinished() => 
            (!this.OnFly || ((Time.timeSinceLevelLoad - this.StartTime) > JUMP_MAX_TIME)) || (this.Slowdown && ((Time.timeSinceLevelLoad - this.SlowdownStartTime) > SLOWDOWN_TIME));

        public bool isNearBegin() => 
            this.OnFly && ((Time.timeSinceLevelLoad - this.StartTime) < NEAR_START_TIME);

        public void StartJump(Vector3 velocity)
        {
            this.StartTime = Time.timeSinceLevelLoad;
            this.Velocity = velocity;
            this.OnFly = true;
            this.Slowdown = false;
            base.OnChange();
        }

        public float StartTime { get; set; }

        public Vector3 Velocity { get; set; }

        public bool OnFly { get; set; }

        public bool Slowdown { get; set; }

        public float SlowdownStartTime { get; set; }
    }
}

