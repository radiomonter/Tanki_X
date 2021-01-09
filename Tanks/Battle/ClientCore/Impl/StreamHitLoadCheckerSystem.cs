namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;

    public class StreamHitLoadCheckerSystem : ECSSystem
    {
        private void AddIfMatches(TankNode tank, StreamHitNode streamHit)
        {
            StreamHitComponent component = streamHit.streamHit;
            if ((component.TankHit != null) && ReferenceEquals(component.TankHit.Entity, tank.Entity))
            {
                streamHit.Entity.AddComponentIfAbsent<StreamHitTargetLoadedComponent>();
            }
        }

        [OnEventComplete]
        public void Remove(NodeRemoveEvent e, LoadedHitForNRNode nr, [JoinSelf] LoadedHitNode streamHit)
        {
            streamHit.Entity.RemoveComponent<StreamHitTargetLoadedComponent>();
        }

        [OnEventFire]
        public void TryMarkTargetLoaded(NodeAddedEvent e, StreamHitNode streamHit, [JoinByBattle] ICollection<TankNode> tanks)
        {
            foreach (TankNode node in tanks)
            {
                this.AddIfMatches(node, streamHit);
            }
        }

        [OnEventFire]
        public void TryMarkTargetLoaded(NodeAddedEvent e, TankNode tank, [JoinByBattle] ICollection<StreamHitNode> streamHits)
        {
            foreach (StreamHitNode node in streamHits)
            {
                this.AddIfMatches(tank, node);
            }
        }

        public class LoadedHitForNRNode : Node
        {
            public StreamHitComponent streamHit;
            public BattleGroupComponent battleGroup;
        }

        public class LoadedHitNode : Node
        {
            public StreamHitComponent streamHit;
            public StreamHitTargetLoadedComponent streamHitTargetLoaded;
            public BattleGroupComponent battleGroup;
        }

        public class StreamHitNode : Node
        {
            public StreamHitComponent streamHit;
            public BattleGroupComponent battleGroup;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public AssembledTankComponent assembledTank;
            public BattleGroupComponent battleGroup;
        }
    }
}

