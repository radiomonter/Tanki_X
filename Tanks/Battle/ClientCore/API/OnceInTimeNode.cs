namespace Tanks.Battle.ClientCore.API
{
    using System;
    using UnityEngine;

    public class OnceInTimeNode : CompositeNode
    {
        public float Time;
        private float timeToFinish;

        public override void End()
        {
        }

        public override TreeNodeState Running()
        {
            if (UnityEngine.Time.timeSinceLevelLoad <= this.timeToFinish)
            {
                return TreeNodeState.RUNNING;
            }
            for (int i = 0; i < base.children.Count; i++)
            {
                base.children[i].Update();
            }
            return TreeNodeState.SUCCESS;
        }

        public override void Start()
        {
            this.timeToFinish = UnityEngine.Time.timeSinceLevelLoad + this.Time;
        }
    }
}

