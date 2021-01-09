namespace Tanks.Battle.ClientCore.API
{
    using System;

    public class SelectorNode : CompositeNode
    {
        public override TreeNodeState Running()
        {
            for (int i = base.currentChildIndex; i < base.children.Count; i++)
            {
                TreeNodeState state = base.children[i].Update();
                if (state != TreeNodeState.FAILURE)
                {
                    base.state = state;
                    base.currentChildIndex = i;
                    for (int j = 0; j < base.children.Count; j++)
                    {
                        if (j != i)
                        {
                            base.children[j].Reset();
                        }
                    }
                    return base.state;
                }
            }
            return TreeNodeState.FAILURE;
        }
    }
}

