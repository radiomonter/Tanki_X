namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MinePhysicsTriggerBehaviour : TriggerBehaviour<TriggerEnterEvent>
    {
        [CompilerGenerated]
        private static Action<Collider> <>f__am$cache0;

        private void OnTriggerEnter(Collider other)
        {
            base.SendEventByCollision(other);
        }

        private void Start()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = c => c.enabled = true;
            }
            base.GetComponentsInChildren<Collider>(true).ForEach<Collider>(<>f__am$cache0);
        }
    }
}

