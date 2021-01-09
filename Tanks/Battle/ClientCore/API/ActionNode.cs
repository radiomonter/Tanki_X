namespace Tanks.Battle.ClientCore.API
{
    using System;

    public class ActionNode : BehaviourTreeNode
    {
        public System.Action Action;

        public override void End()
        {
        }

        public override TreeNodeState Running()
        {
            this.Action();
            return TreeNodeState.SUCCESS;
        }

        public override void Start()
        {
        }
    }
}

