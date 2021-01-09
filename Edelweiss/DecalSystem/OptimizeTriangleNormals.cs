namespace Edelweiss.DecalSystem
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;

    internal class OptimizeTriangleNormals
    {
        private SortedDictionary<int, Vector3> m_TriangleIndexToNormalDictionary = new SortedDictionary<int, Vector3>();

        public void AddTriangleNormal(int a_TriangleIndex, Vector3 a_TriangleNormal)
        {
            this.m_TriangleIndexToNormalDictionary.Add(a_TriangleIndex, a_TriangleNormal);
        }

        public void Clear()
        {
            this.m_TriangleIndexToNormalDictionary.Clear();
        }

        public bool HasTriangleNormal(int a_TriangleIndex) => 
            this.m_TriangleIndexToNormalDictionary.ContainsKey(a_TriangleIndex);

        public void RemoveTriangleNormal(int a_TriangleIndex)
        {
            this.m_TriangleIndexToNormalDictionary.Remove(a_TriangleIndex);
        }

        public int Count =>
            this.m_TriangleIndexToNormalDictionary.Count;

        public Vector3 this[int a_TriangleIndex] =>
            this.m_TriangleIndexToNormalDictionary[a_TriangleIndex];
    }
}

