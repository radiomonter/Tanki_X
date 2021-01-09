namespace Tanks.Battle.ClientCore.API
{
    using System;

    public abstract class BehaviourTreeNode
    {
        public TreeNodeState state;

        protected BehaviourTreeNode()
        {
        }

        public abstract void End();
        public void Reset()
        {
            this.state = TreeNodeState.NONE;
        }

        public abstract TreeNodeState Running();
        public abstract void Start();
        public TreeNodeState Update()
        {
            if (this.state != TreeNodeState.RUNNING)
            {
                this.Start();
            }
            this.state = this.Running();
            if (this.state != this.Running())
            {
                this.End();
            }
            return this.state;
        }
    }
}

