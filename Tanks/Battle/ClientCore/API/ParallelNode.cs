namespace Tanks.Battle.ClientCore.API
{
    using System;

    public class ParallelNode : CompositeNode
    {
        public override TreeNodeState Running()
        {
            for (int i = 0; i < base.children.Count; i++)
            {
                base.children[i].Update();
            }
            return TreeNodeState.SUCCESS;
        }
    }
}

