namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectSetPositionOnHit : MonoBehaviour
    {
        public float OffsetPosition;
        private UpdateRankEffectSettings effectSettings;
        private Transform tRoot;
        private bool isInitialized;

        private void effectSettings_CollisionEnter(object sender, UpdateRankCollisionInfo e)
        {
            Vector3 normalized = (this.tRoot.position + (Vector3.Normalize(e.Hit.point - this.tRoot.position) * (this.effectSettings.MoveDistance + 1f))).normalized;
            base.transform.position = e.Hit.point - (normalized * this.OffsetPosition);
        }

        private void GetEffectSettingsComponent(Transform tr)
        {
            Transform parent = tr.parent;
            if (parent != null)
            {
                this.effectSettings = parent.GetComponentInChildren<UpdateRankEffectSettings>();
                if (this.effectSettings == null)
                {
                    this.GetEffectSettingsComponent(parent.transform);
                }
            }
        }

        private void OnDisable()
        {
            base.transform.position = Vector3.zero;
        }

        private void Start()
        {
            this.GetEffectSettingsComponent(base.transform);
            if (this.effectSettings == null)
            {
                Debug.Log("Prefab root or children have not script \"PrefabSettings\"");
            }
            this.tRoot = this.effectSettings.transform;
        }

        private void Update()
        {
            if (!this.isInitialized)
            {
                this.isInitialized = true;
                this.effectSettings.CollisionEnter += new EventHandler<UpdateRankCollisionInfo>(this.effectSettings_CollisionEnter);
            }
        }
    }
}

