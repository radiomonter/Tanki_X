namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    public class InternalPreciseTime
    {
        public static void AfterFixedUpdate()
        {
            PreciseTime.AfterFixedUpdate();
        }

        public static void FixedUpdate(float fixedDeltaTime)
        {
            PreciseTime.FixedUpdate(fixedDeltaTime);
        }

        public static void Update(float deltaTime)
        {
            PreciseTime.Update(deltaTime);
        }
    }
}

