namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Runtime.InteropServices;

    public class ConditionNode : BehaviourTreeNode
    {
        public Func<bool> Condition;
        public string Name;

        public ConditionNode(string name = "")
        {
            this.Name = name;
        }

        public override void End()
        {
        }

        public override TreeNodeState Running() => 
            !this.Condition() ? TreeNodeState.FAILURE : TreeNodeState.SUCCESS;

        public override void Start()
        {
        }
    }
}

