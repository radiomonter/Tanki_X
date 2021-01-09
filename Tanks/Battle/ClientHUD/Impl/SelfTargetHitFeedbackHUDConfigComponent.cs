namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class SelfTargetHitFeedbackHUDConfigComponent : BehaviourComponent
    {
        private const float Z_VALUE = 0.5f;
        private const float LENGTH = 10f;
        [SerializeField]
        private SelfTargetHitFeedbackHUDInstanceComponent effectPrefab;
        [SerializeField]
        private BoxCollider coliderHelper;

        public Vector2? GetBoundPosition(Vector3 targetPos, Vector2 hitVecViewportSpace)
        {
            RaycastHit hit;
            Vector3 normalized = this.coliderHelper.transform.TransformVector(new Vector3(hitVecViewportSpace.x, hitVecViewportSpace.y, 0f)).normalized;
            Ray ray = new Ray(this.coliderHelper.transform.TransformPoint(new Vector3(targetPos.x, targetPos.y, 0.5f)) - (normalized * 10f), normalized);
            if (!this.coliderHelper.Raycast(ray, out hit, 10f))
            {
                return null;
            }
            Vector3 vector5 = this.coliderHelper.transform.InverseTransformPoint(hit.point);
            return new Vector2?(new Vector2(vector5.x, vector5.y));
        }

        public SelfTargetHitFeedbackHUDInstanceComponent EffectPrefab =>
            this.effectPrefab;
    }
}

