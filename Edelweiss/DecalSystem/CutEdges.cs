namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal class CutEdges
    {
        private SortedDictionary<CutEdge, CutEdge> m_CutEdgeDictionary = new SortedDictionary<CutEdge, CutEdge>();

        public void AddEdge(CutEdge a_CutEdge)
        {
            this.m_CutEdgeDictionary.Add(a_CutEdge, a_CutEdge);
        }

        public void Clear()
        {
            this.m_CutEdgeDictionary.Clear();
        }

        public bool HasEdge(CutEdge a_CutEdge) => 
            this.m_CutEdgeDictionary.ContainsKey(a_CutEdge);

        public int Count =>
            this.m_CutEdgeDictionary.Count;

        public CutEdge this[CutEdge a_CutEdge]
        {
            get => 
                this.m_CutEdgeDictionary[a_CutEdge];
            set => 
                this.m_CutEdgeDictionary[a_CutEdge] = value;
        }
    }
}

