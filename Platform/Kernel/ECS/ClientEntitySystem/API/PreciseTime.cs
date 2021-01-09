namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PreciseTime
    {
        internal static void AfterFixedUpdate()
        {
            TimeType = Platform.Kernel.ECS.ClientEntitySystem.API.TimeType.LAST_FIXED;
        }

        internal static void FixedUpdate(float fixedDeltaTime)
        {
            TimeType = Platform.Kernel.ECS.ClientEntitySystem.API.TimeType.FIXED;
        }

        internal static void Update(float deltaTime)
        {
            TimeType = Platform.Kernel.ECS.ClientEntitySystem.API.TimeType.UPDATE;
        }

        public static double Time =>
            (double) UnityEngine.Time.timeSinceLevelLoad;

        public static Platform.Kernel.ECS.ClientEntitySystem.API.TimeType TimeType { get; private set; }
    }
}

