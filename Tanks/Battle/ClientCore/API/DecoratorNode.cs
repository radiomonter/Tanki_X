namespace Tanks.Battle.ClientCore.API
{
    using System;

    public abstract class DecoratorNode : BehaviourTreeNode
    {
        public BehaviourTreeNode Child;

        protected DecoratorNode()
        {
        }

        public void AddChild(BehaviourTreeNode child)
        {
            this.Child = child;
        }
    }
}

