namespace Tanks.Battle.ClientGraphics.Impl
{
    using LeopotamGroup.Pooling;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class PoolObjectExtensions
    {
        public static void RecycleObject(this GameObject gameObject)
        {
            if (gameObject)
            {
                gameObject.transform.rotation = Quaternion.identity;
                IPoolObject component = gameObject.GetComponent<IPoolObject>();
                if (component != null)
                {
                    component.PoolRecycle(true);
                }
                else
                {
                    Object.Destroy(gameObject);
                }
            }
        }
    }
}

