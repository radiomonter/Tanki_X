namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;

    public class MoveToTargetNode : BehaviourTreeNode
    {
        public TankAutopilotControllerSystem.AutopilotTankNode tank;

        public override void End()
        {
        }

        public override TreeNodeState Running() => 
            TreeNodeState.RUNNING;

        public override void Start()
        {
        }

        private float LastMove { get; set; }

        private float LastTurn { get; set; }
    }
}

