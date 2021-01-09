namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class SplashHitData
    {
        private const string TARGETS_ARRAY_DELIMETER = ", ";
        private const string LOG_FORMAT = "Splash Hit Data: direct targets = [{0}] ; static hit = {1} ; splash center = {2} splash targets = [{3}]";
        private List<HitTarget> directTargets;
        private Tanks.Battle.ClientCore.API.StaticHit staticHit;
        private Entity weaponHitEntity;
        private List<HitTarget> splashTargets;
        private List<GameObject> exclusionGameObjectForSplashRaycast;
        private Vector3 splashCenter;
        private bool splashCenterInitialized = false;
        [CompilerGenerated]
        private static Func<HitTarget, string> <>f__am$cache0;

        private SplashHitData()
        {
        }

        private string ConvertTargetsCollectionToString(List<HitTarget> targets)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = i => i.ToString();
            }
            return string.Join(", ", targets.Select<HitTarget, string>(<>f__am$cache0).ToArray<string>());
        }

        public static SplashHitData CreateSplashHitData(List<HitTarget> directTargets, Tanks.Battle.ClientCore.API.StaticHit staticHit, Entity weaponHitEntity) => 
            new SplashHitData { 
                directTargets = directTargets,
                staticHit = staticHit,
                weaponHitEntity = weaponHitEntity,
                splashTargets = new List<HitTarget>(),
                exclusionGameObjectForSplashRaycast = new List<GameObject>(),
                splashCenter = Vector3.zero,
                ExcludedEntityForSplashHit = null
            };

        public override string ToString()
        {
            object[] objArray1 = new object[4];
            objArray1[0] = this.ConvertTargetsCollectionToString(this.directTargets);
            objArray1[1] = (this.staticHit != null) ? this.staticHit.ToString() : string.Empty;
            object[] args = objArray1;
            args[2] = this.splashCenter.ToString();
            args[3] = this.ConvertTargetsCollectionToString(this.splashTargets);
            return string.Format("Splash Hit Data: direct targets = [{0}] ; static hit = {1} ; splash center = {2} splash targets = [{3}]", args);
        }

        public List<HitTarget> DirectTargets =>
            this.directTargets;

        public Tanks.Battle.ClientCore.API.StaticHit StaticHit =>
            this.staticHit;

        public Entity WeaponHitEntity =>
            this.weaponHitEntity;

        public List<HitTarget> SplashTargets =>
            this.splashTargets;

        public List<GameObject> ExclusionGameObjectForSplashRaycast =>
            this.exclusionGameObjectForSplashRaycast;

        public Vector3 SplashCenter
        {
            get => 
                this.splashCenter;
            set
            {
                this.splashCenterInitialized = true;
                this.splashCenter = value;
            }
        }

        public HashSet<Entity> ExcludedEntityForSplashHit { get; set; }

        public bool SplashCenterInitialized =>
            this.splashCenterInitialized;
    }
}

