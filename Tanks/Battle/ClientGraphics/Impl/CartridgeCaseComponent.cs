namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class CartridgeCaseComponent : BehaviourComponent
    {
        public float lifeTime = 5f;
        private Collider collider;
        private bool _selfDestructionStarted;

        private void DestroyCase()
        {
            base.gameObject.RecycleObject();
            this._selfDestructionStarted = false;
        }

        private void OnEnable()
        {
            this.collider = base.GetComponent<Collider>();
        }

        public void StartSelfDestruction()
        {
            if (!this._selfDestructionStarted)
            {
                this._selfDestructionStarted = true;
                base.Invoke("DestroyCase", this.lifeTime);
                this.collider = base.GetComponent<Collider>();
                this.collider.isTrigger = false;
            }
        }
    }
}

