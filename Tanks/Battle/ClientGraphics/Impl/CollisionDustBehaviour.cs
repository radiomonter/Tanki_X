namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class CollisionDustBehaviour : MonoBehaviour
    {
        [NonSerialized]
        public MapDustComponent mapDust;
        private float delay;
        private DustEffectBehaviour effect;

        private void OnCollisionStay(Collision collision)
        {
            this.effect = this.mapDust.GetEffectByTag(collision.transform, Vector2.zero);
            if ((this.effect != null) && (this.delay <= 0f))
            {
                this.delay = 1f / this.effect.collisionEmissionRate.RandomValue;
                foreach (ContactPoint point in collision.contacts)
                {
                    this.effect.TryEmitParticle(point.point, collision.relativeVelocity);
                }
            }
        }

        private void Update()
        {
            this.delay -= Time.deltaTime;
        }

        public DustEffectBehaviour Effect =>
            this.effect;
    }
}

