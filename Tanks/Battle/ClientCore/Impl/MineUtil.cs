namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public static class MineUtil
    {
        public static readonly float TANK_MINE_RAYCAST_DISTANCE = 10000f;

        public static void ExecuteSplashExplosion(Entity mine, Entity tank, Vector3 splashCenter)
        {
            StaticHit staticHit = new StaticHit {
                Normal = Vector3.up,
                Position = splashCenter
            };
            SplashHitData splashHit = SplashHitData.CreateSplashHitData(new List<HitTarget>(), staticHit, mine);
            HashSet<Entity> set = new HashSet<Entity> {
                tank
            };
            splashHit.ExcludedEntityForSplashHit = set;
            EngineService.Engine.ScheduleEvent<SendTankMovementEvent>(tank);
            EngineService.Engine.ScheduleEvent(new CollectSplashTargetsEvent(splashHit), mine);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }
    }
}

