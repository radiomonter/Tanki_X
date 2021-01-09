namespace LeopotamGroup.Pooling
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class PoolObject : MonoBehaviour, IPoolObject
    {
        private LeopotamGroup.Pooling.PoolContainer _container;

        public virtual void PoolRecycle(bool checkDoubleRecycles = true)
        {
            if (this._container != null)
            {
                this._container.Recycle(this, checkDoubleRecycles);
            }
        }

        public virtual LeopotamGroup.Pooling.PoolContainer PoolContainer
        {
            get => 
                this._container;
            set => 
                this._container = value;
        }

        public virtual Transform PoolTransform =>
            base.transform;
    }
}

