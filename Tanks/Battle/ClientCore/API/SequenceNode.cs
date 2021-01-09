namespace Tanks.Battle.ClientCore.API
{
    using System;

    public class SequenceNode : CompositeNode
    {
        public override TreeNodeState Running()
        {
            for (int i = base.currentChildIndex; i < base.children.Count; i++)
            {
                TreeNodeState state = base.children[i].Update();
                if (state != TreeNodeState.SUCCESS)
                {
                    base.state = state;
                    base.currentChildIndex = i;
                    return base.state;
                }
            }
            return TreeNodeState.SUCCESS;
        }
    }
}

