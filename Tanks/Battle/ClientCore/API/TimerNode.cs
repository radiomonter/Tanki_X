namespace Tanks.Battle.ClientCore.API
{
    using System;
    using UnityEngine;

    public class TimerNode : DecoratorNode
    {
        public float Time;
        private float timeToFinish;

        public override void End()
        {
        }

        public override TreeNodeState Running()
        {
            if (UnityEngine.Time.timeSinceLevelLoad > this.timeToFinish)
            {
                return TreeNodeState.SUCCESS;
            }
            base.Child.Update();
            return TreeNodeState.RUNNING;
        }

        public override void Start()
        {
            this.timeToFinish = UnityEngine.Time.timeSinceLevelLoad + this.Time;
        }
    }
}

