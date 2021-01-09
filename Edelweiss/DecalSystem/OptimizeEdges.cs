namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal class OptimizeEdges
    {
        private SortedDictionary<OptimizeEdge, OptimizeEdge> m_EdgeDictionary = new SortedDictionary<OptimizeEdge, OptimizeEdge>();

        public void AddEdge(OptimizeEdge a_OptimizeEdge)
        {
            this.m_EdgeDictionary.Add(a_OptimizeEdge, a_OptimizeEdge);
        }

        public void Clear()
        {
            this.m_EdgeDictionary.Clear();
        }

        public bool HasEdge(OptimizeEdge a_OptimizeEdge) => 
            this.m_EdgeDictionary.ContainsKey(a_OptimizeEdge);

        public List<OptimizeEdge> OptimizedEdgeList()
        {
            List<OptimizeEdge> list = new List<OptimizeEdge>();
            foreach (OptimizeEdge edge in this.m_EdgeDictionary.Keys)
            {
                list.Add(edge);
            }
            return list;
        }

        public void RemoveEdge(OptimizeEdge a_OptimizeEdge)
        {
            this.m_EdgeDictionary.Remove(a_OptimizeEdge);
        }

        public int Count =>
            this.m_EdgeDictionary.Count;

        public OptimizeEdge this[OptimizeEdge a_OptimizeEdge]
        {
            get => 
                this.m_EdgeDictionary[a_OptimizeEdge];
            set => 
                this.m_EdgeDictionary[a_OptimizeEdge] = value;
        }
    }
}

