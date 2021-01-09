namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Collections.Generic;

    public abstract class CompositeNode : BehaviourTreeNode
    {
        public List<BehaviourTreeNode> children = new List<BehaviourTreeNode>();
        public int currentChildIndex;

        protected CompositeNode()
        {
        }

        public void AddChild(BehaviourTreeNode child)
        {
            this.children.Add(child);
        }

        public override void End()
        {
            this.currentChildIndex = 0;
        }

        public void Reset()
        {
            base.Reset();
            this.currentChildIndex = 0;
            for (int i = 0; i < this.children.Count; i++)
            {
                this.children[i].Reset();
            }
        }

        public override void Start()
        {
            this.currentChildIndex = 0;
        }
    }
}

