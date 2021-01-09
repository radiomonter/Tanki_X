namespace Tanks.Battle.ClientCore.API
{
    using System;

    public class PreconditionSequence : CompositeNode
    {
        public override TreeNodeState Running()
        {
            for (int i = 0; i < base.children.Count; i++)
            {
                TreeNodeState state = base.children[i].Update();
                if (state != TreeNodeState.SUCCESS)
                {
                    base.state = state;
                    return base.state;
                }
            }
            return TreeNodeState.SUCCESS;
        }
    }
}

