namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectDeadTime : MonoBehaviour
    {
        public float deadTime = 1.5f;
        public bool destroyRoot;

        private void Awake()
        {
            Destroy(this.destroyRoot ? base.transform.root.gameObject : base.gameObject, this.deadTime);
        }
    }
}

