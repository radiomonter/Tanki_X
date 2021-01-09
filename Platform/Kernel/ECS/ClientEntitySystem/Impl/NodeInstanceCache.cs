namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class NodeInstanceCache
    {
        private const int MAX_CACHE_SIZE = 100;
        private Stack<Node> cache = new Stack<Node>();
        private List<Node> freedInflow = new List<Node>();
        private Type nodeType;

        public NodeInstanceCache(Type nodeType)
        {
            this.nodeType = nodeType;
        }

        public void Free(Node item)
        {
            if (this.freedInflow.Count <= 100)
            {
                this.freedInflow.Add(item);
            }
        }

        public Node GetInstance() => 
            (this.cache.Count == 0) ? ((Node) Activator.CreateInstance(this.nodeType)) : this.cache.Pop();

        public void OnFlowClean()
        {
            for (int i = 0; (i < this.freedInflow.Count) && (this.cache.Count <= 100); i++)
            {
                this.cache.Push(this.freedInflow[i]);
            }
            this.freedInflow.Clear();
        }
    }
}

