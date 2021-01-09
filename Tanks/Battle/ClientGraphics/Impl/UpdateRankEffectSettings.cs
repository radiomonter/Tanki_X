namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class UpdateRankEffectSettings : MonoBehaviour
    {
        public RankIconComponent icon;
        [Tooltip("Type of the effect")]
        public EffectTypeEnum EffectType;
        [Tooltip("The radius of the collider is required to correctly calculate the collision point. For example, if the radius 0.5m, then the position of the collision is shifted on 0.5m relative motion vector.")]
        public float ColliderRadius = 0.2f;
        [Tooltip("The radius of the \"Area Of Damage (AOE)\"")]
        public float EffectRadius;
        [Tooltip("Get the position of the movement of the motion vector, and not to follow to the target.")]
        public bool UseMoveVector;
        [Tooltip("A projectile will be moved to the target (any object)")]
        public GameObject Target;
        [Tooltip("Motion vector for the projectile (eg Vector3.Forward)")]
        public Vector3 MoveVector = Vector3.forward;
        [Tooltip("The speed of the projectile")]
        public float MoveSpeed = 1f;
        [Tooltip("Should the projectile have move to the target, until the target not reaches?")]
        public bool IsHomingMove;
        [Tooltip("Distance flight of the projectile, after which the projectile is deactivated and call a collision event with a null value \"RaycastHit\"")]
        public float MoveDistance = 20f;
        [Tooltip("Allows you to smoothly activate / deactivate effects which have an indefinite lifetime")]
        public bool IsVisible = true;
        [Tooltip("Whether to deactivate or destroy the effect after a collision. Deactivation allows you to reuse the effect without instantiating, using \"effect.SetActive (true)\"")]
        public DeactivationEnum InstanceBehaviour = DeactivationEnum.Nothing;
        [Tooltip("Delay before deactivating effect. (For example, after effect, some particles must have time to disappear).")]
        public float DeactivateTimeDelay = 4f;
        [Tooltip("Delay before deleting effect. (For example, after effect, some particles must have time to disappear).")]
        public float DestroyTimeDelay = 10f;
        [Tooltip("Allows you to adjust the layers, which can interact with the projectile.")]
        public UnityEngine.LayerMask LayerMask = -1;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<UpdateRankCollisionInfo> CollisionEnter;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler EffectDeactivated;
        private GameObject[] active_key = new GameObject[100];
        private float[] active_value = new float[100];
        private GameObject[] inactive_Key = new GameObject[100];
        private float[] inactive_value = new float[100];
        private int lastActiveIndex;
        private int lastInactiveIndex;
        private int currentActiveGo;
        private int currentInactiveGo;
        private bool deactivatedIsWait;

        public event EventHandler<UpdateRankCollisionInfo> CollisionEnter
        {
            add
            {
                EventHandler<UpdateRankCollisionInfo> collisionEnter = this.CollisionEnter;
                while (true)
                {
                    EventHandler<UpdateRankCollisionInfo> objB = collisionEnter;
                    collisionEnter = Interlocked.CompareExchange<EventHandler<UpdateRankCollisionInfo>>(ref this.CollisionEnter, objB + value, collisionEnter);
                    if (ReferenceEquals(collisionEnter, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<UpdateRankCollisionInfo> collisionEnter = this.CollisionEnter;
                while (true)
                {
                    EventHandler<UpdateRankCollisionInfo> objB = collisionEnter;
                    collisionEnter = Interlocked.CompareExchange<EventHandler<UpdateRankCollisionInfo>>(ref this.CollisionEnter, objB - value, collisionEnter);
                    if (ReferenceEquals(collisionEnter, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler EffectDeactivated
        {
            add
            {
                EventHandler effectDeactivated = this.EffectDeactivated;
                while (true)
                {
                    EventHandler objB = effectDeactivated;
                    effectDeactivated = Interlocked.CompareExchange<EventHandler>(ref this.EffectDeactivated, objB + value, effectDeactivated);
                    if (ReferenceEquals(effectDeactivated, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler effectDeactivated = this.EffectDeactivated;
                while (true)
                {
                    EventHandler objB = effectDeactivated;
                    effectDeactivated = Interlocked.CompareExchange<EventHandler>(ref this.EffectDeactivated, objB - value, effectDeactivated);
                    if (ReferenceEquals(effectDeactivated, objB))
                    {
                        return;
                    }
                }
            }
        }

        public void Deactivate()
        {
            this.OnEffectDeactivatedHandler();
            base.gameObject.SetActive(false);
        }

        public void OnCollisionHandler(UpdateRankCollisionInfo e)
        {
            for (int i = 0; i < this.lastActiveIndex; i++)
            {
                base.Invoke("SetGoActive", this.active_value[i]);
            }
            for (int j = 0; j < this.lastInactiveIndex; j++)
            {
                base.Invoke("SetGoInactive", this.inactive_value[j]);
            }
            EventHandler<UpdateRankCollisionInfo> collisionEnter = this.CollisionEnter;
            if (collisionEnter != null)
            {
                collisionEnter(this, e);
            }
            if ((this.InstanceBehaviour == DeactivationEnum.Deactivate) && !this.deactivatedIsWait)
            {
                this.deactivatedIsWait = true;
                base.Invoke("Deactivate", this.DeactivateTimeDelay);
            }
        }

        public void OnDisable()
        {
            base.CancelInvoke("SetGoActive");
            base.CancelInvoke("SetGoInactive");
            base.CancelInvoke("Deactivate");
            this.currentActiveGo = 0;
            this.currentInactiveGo = 0;
        }

        public void OnEffectDeactivatedHandler()
        {
            EventHandler effectDeactivated = this.EffectDeactivated;
            if (effectDeactivated != null)
            {
                effectDeactivated(this, EventArgs.Empty);
            }
        }

        public void OnEnable()
        {
            for (int i = 0; i < this.lastActiveIndex; i++)
            {
                this.active_key[i].SetActive(true);
            }
            for (int j = 0; j < this.lastInactiveIndex; j++)
            {
                this.inactive_Key[j].SetActive(false);
            }
            this.deactivatedIsWait = false;
        }

        public void RegistreActiveElement(GameObject go, float time)
        {
            this.active_key[this.lastActiveIndex] = go;
            this.active_value[this.lastActiveIndex] = time;
            this.lastActiveIndex++;
        }

        public void RegistreInactiveElement(GameObject go, float time)
        {
            this.inactive_Key[this.lastInactiveIndex] = go;
            this.inactive_value[this.lastInactiveIndex] = time;
            this.lastInactiveIndex++;
        }

        private void SetGoActive()
        {
            this.active_key[this.currentActiveGo].SetActive(false);
            this.currentActiveGo++;
            if (this.currentActiveGo >= this.lastActiveIndex)
            {
                this.currentActiveGo = 0;
            }
        }

        private void SetGoInactive()
        {
            this.inactive_Key[this.currentInactiveGo].SetActive(true);
            this.currentInactiveGo++;
            if (this.currentInactiveGo >= this.lastInactiveIndex)
            {
                this.currentInactiveGo = 0;
            }
        }

        public enum DeactivationEnum
        {
            Deactivate,
            DestroyAfterCollision,
            DestroyAfterTime,
            Nothing
        }

        public enum EffectTypeEnum
        {
            Projectile,
            AOE,
            Other
        }
    }
}

