namespace LeopotamGroup.Pooling
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public interface IPoolObject
    {
        void PoolRecycle(bool checkForDoubleRecycle = true);

        LeopotamGroup.Pooling.PoolContainer PoolContainer { get; set; }

        Transform PoolTransform { get; }
    }
}

