namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;

    public class BattleFlowInstancesCache : AbstratFlowInstancesCache
    {
        public readonly Cache<DirectionData> directionData;
        public readonly Cache<TargetingData> targetingData;
        public readonly Cache<TargetData> targetData;
        public readonly Cache<TargetingEvent> targetingEvent;
        public readonly Cache<TargetingEvaluateEvent> targetEvaluateEvent;
        public readonly Cache<CollectDirectionsEvent> collectDirectionsEvent;
        public readonly Cache<CollectTargetsEvent> collectTargetsEvent;
        public readonly Cache<UpdateBulletEvent> updateBulletEvent;
        public readonly Cache<LinkedList<TargetSector>> targetSectors;
        public readonly Cache<CollectTargetSectorsEvent> collectTargetSectorsEvent;
        public readonly Cache<CollectSectorDirectionsEvent> collectSectorDirectionsEvent;

        public BattleFlowInstancesCache()
        {
            this.targetingData = base.Register<TargetingData>();
            this.directionData = base.Register<DirectionData>();
            this.targetData = base.Register<TargetData>();
            this.targetingEvent = base.Register<TargetingEvent>();
            this.targetEvaluateEvent = base.Register<TargetingEvaluateEvent>();
            this.collectDirectionsEvent = base.Register<CollectDirectionsEvent>();
            this.collectTargetsEvent = base.Register<CollectTargetsEvent>();
            this.updateBulletEvent = base.Register<UpdateBulletEvent>();
            this.targetSectors = base.Register<LinkedList<TargetSector>>();
            this.collectTargetSectorsEvent = base.Register<CollectTargetSectorsEvent>();
            this.collectSectorDirectionsEvent = base.Register<CollectSectorDirectionsEvent>();
        }
    }
}

