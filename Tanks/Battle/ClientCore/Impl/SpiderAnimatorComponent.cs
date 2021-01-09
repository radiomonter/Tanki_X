namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class SpiderAnimatorComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private bool runiningOnStart;
        [SerializeField]
        private string activationClipName = "Activation";
        [SerializeField]
        private string runClipName = "Walk";
        [SerializeField]
        private float mass = 1f;
        [SerializeField]
        private float runAnimationSpeed = 1f;
        [SerializeField]
        private float runForce = 10f;
        [SerializeField]
        private float rotationSpeed = 10f;
        [SerializeField]
        private float maximalRuningSpeed = 10f;
        [SerializeField]
        private float runingDrag = 1f;
        [SerializeField]
        private string jumpClipName = "Jump";
        [SerializeField]
        private float jumpForce = 500f;
        [SerializeField]
        private float maxDepenetrationVelocity = 2f;
        [SerializeField]
        private string idleClipName = "Idle";
        private bool onGround;
        private bool jumpForceApplied;
        private SpiderAnimationState currentState;
        private SpiderAnimationState oldState;
        private Dictionary<SpiderAnimationState, SpiderAnimationEntry> states = new Dictionary<SpiderAnimationState, SpiderAnimationEntry>();
        private Rigidbody rigidbody;
        private Animation animation;
        private Rigidbody targetBody;

        private void Activation()
        {
            SpiderAnimationEntry entry = this.states[this.currentState];
            if ((this.targetBody != null) && ((entry.Animation.normalizedTime % 1f) >= 0.99f))
            {
                this.StartRuning();
            }
        }

        private void AddStabilizeForce()
        {
            float num = 0.0005f;
            if (Vector3.Dot(this.rigidbody.transform.up, Vector3.up) < 0.2f)
            {
                num = 0.2f;
            }
            Quaternion quaternion = Quaternion.FromToRotation(this.rigidbody.transform.up, Vector3.up);
            this.rigidbody.AddTorqueSafe(quaternion.eulerAngles * num);
        }

        private void ConfigureState(SpiderAnimationState animationState, string clipName, Action action = null)
        {
            SpiderAnimationEntry entry = new SpiderAnimationEntry {
                ClipName = clipName,
                Animation = this.animation[clipName],
                Action = action
            };
            this.states.Add(animationState, entry);
        }

        private void FixedUpdate()
        {
            this.AddStabilizeForce();
            if (this.states.ContainsKey(this.currentState))
            {
                if (this.oldState != this.currentState)
                {
                    this.animation.CrossFade(this.states[this.currentState].ClipName);
                    this.oldState = this.currentState;
                    this.states[this.currentState].StartTime = Time.timeSinceLevelLoad;
                }
                if (this.states[this.currentState].Action != null)
                {
                    this.states[this.currentState].Action();
                }
            }
        }

        private Vector3 GetDirectionToTarget()
        {
            Vector3 zero = Vector3.zero;
            if (this.runiningOnStart)
            {
                zero = Vector3.forward;
            }
            if (this.targetBody)
            {
                zero = (this.targetBody.position - this.rigidbody.position).normalized;
            }
            return zero;
        }

        private void Jumping()
        {
            SpiderAnimationEntry entry = this.states[this.currentState];
            entry.Animation.speed = 1.3f;
            if (!this.jumpForceApplied && ((entry.Animation.normalizedTime % 1f) > 0.3f))
            {
                Vector3 normalized = ((this.GetDirectionToTarget() * 0.2f) + (Vector3.up * 0.8f)).normalized;
                this.rigidbody.AddForceSafe(normalized * this.jumpForce);
                this.jumpForceApplied = true;
            }
            if ((entry.Animation.normalizedTime % 1f) > 0.99f)
            {
                this.StartRuning();
            }
        }

        private void Runing()
        {
            SpiderAnimationEntry entry = this.states[this.currentState];
            Vector3 directionToTarget = this.GetDirectionToTarget();
            if (directionToTarget.Equals(Vector3.zero))
            {
                entry.Animation.speed = 0f;
            }
            else
            {
                RaycastHit hit;
                float num = Vector3.Dot(directionToTarget, this.rigidbody.transform.right);
                float num2 = 0f;
                if (num > 0f)
                {
                    num2 = 1f;
                }
                if (num < 0f)
                {
                    num2 = -1f;
                }
                this.onGround = Physics.Raycast(base.transform.position + (base.transform.up * 0.2f), -base.transform.up, out hit, 1f, LayerMasks.STATIC);
                if (!this.onGround)
                {
                    entry.Animation.speed = (Vector3.Dot(this.rigidbody.transform.up, Vector3.up) >= 0.1f) ? 0.2f : 4f;
                    this.rigidbody.drag = 0f;
                }
                else
                {
                    float magnitude = this.rigidbody.velocity.magnitude;
                    if (magnitude > this.maximalRuningSpeed)
                    {
                        this.rigidbody.SetVelocitySafe((this.rigidbody.velocity * this.maximalRuningSpeed) / magnitude);
                    }
                    float num4 = Vector3.ProjectOnPlane(this.rigidbody.velocity, hit.normal).magnitude;
                    entry.Animation.speed = num4 * this.runAnimationSpeed;
                    Vector3 normalized = directionToTarget;
                    if (Vector3.Dot(hit.normal, Vector3.up) > 0.5f)
                    {
                        normalized = Vector3.ProjectOnPlane(directionToTarget, hit.normal).normalized;
                    }
                    normalized = (normalized + (Vector3.up * 0.2f)).normalized;
                    this.rigidbody.AddTorqueSafe(0f, num2 * this.rotationSpeed, 0f);
                    this.rigidbody.AddForceAtPositionSafe(normalized * this.runForce, base.transform.position);
                    this.rigidbody.drag = this.runingDrag;
                }
            }
        }

        public void SetTarget(Rigidbody target)
        {
            this.targetBody = target;
        }

        private void Start()
        {
            this.rigidbody = base.GetComponentInParent<Rigidbody>();
            this.animation = base.GetComponentInParent<Animation>();
            this.rigidbody.mass = this.mass;
            this.rigidbody.maxDepenetrationVelocity = this.maxDepenetrationVelocity;
            this.ConfigureState(SpiderAnimationState.ACTIVATION, this.activationClipName, new Action(this.Activation));
            this.ConfigureState(SpiderAnimationState.IDLE, this.idleClipName, null);
            this.ConfigureState(SpiderAnimationState.RUN, this.runClipName, new Action(this.Runing));
            this.ConfigureState(SpiderAnimationState.JUMP, this.jumpClipName, new Action(this.Jumping));
            if (this.runiningOnStart)
            {
                this.StartRuning();
            }
        }

        public void StartActivation()
        {
            this.currentState = SpiderAnimationState.ACTIVATION;
        }

        public void StartIdle()
        {
            this.currentState = SpiderAnimationState.IDLE;
        }

        public void StartJump()
        {
            this.currentState = SpiderAnimationState.JUMP;
            this.jumpForceApplied = false;
        }

        public void StartRuning()
        {
            this.onGround = true;
            this.currentState = SpiderAnimationState.RUN;
        }

        public float Speed
        {
            set => 
                this.maximalRuningSpeed = value;
        }

        public float Drag
        {
            set => 
                this.runingDrag = value;
        }

        public float Acceleration
        {
            set => 
                this.runForce = this.mass * value;
        }

        public bool OnGround =>
            this.onGround;

        public class SpiderAnimationEntry
        {
            public string ClipName { get; set; }

            public AnimationState Animation { get; set; }

            public System.Action Action { get; set; }

            public float StartTime { get; set; }

            public float TimeSinceStart =>
                Time.timeSinceLevelLoad - this.StartTime;
        }

        public enum SpiderAnimationState
        {
            DISABLED,
            ACTIVATION,
            IDLE,
            RUN,
            JUMP,
            ATTACK_JUMP
        }
    }
}

