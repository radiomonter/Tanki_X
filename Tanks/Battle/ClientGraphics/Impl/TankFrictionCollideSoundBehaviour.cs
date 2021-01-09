namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankFrictionCollideSoundBehaviour : MonoBehaviour
    {
        private const float COLLIDE_ANGLE_THRESHOLD = 45f;
        private const float MIN_VALUABLE_SOUND_VOLUME = 0.1f;
        private const float RAYCAST_EXTRA_LENGTH = 0.25f;
        private const int CONTACT_LIMIT_COUNT = 2;
        private Vector3 velocity;
        private Vector3 previousVelocity;
        private Vector3 angularVelocity;
        private Vector3 previousAngularVelocity;
        private Rigidbody rigidbody;
        private SoundController contactSoundController;
        private float minCollisionPower;
        private float maxCollisionPower;
        private float halfLength;

        private void Awake()
        {
            base.enabled = false;
        }

        private float CalculateVolumeByVelocity(Vector3 velocity, float min, float max) => 
            Mathf.Clamp01((velocity.sqrMagnitude - min) / (max - min));

        private void FixedUpdate()
        {
            this.previousVelocity = this.velocity;
            this.velocity = this.rigidbody.velocity;
            this.previousAngularVelocity = this.angularVelocity;
            this.angularVelocity = this.rigidbody.angularVelocity;
        }

        public void Init(SoundController contactSoundController, Rigidbody rigidbody, float halfLength, float minCollisionPower, float maxCollisionPower)
        {
            this.contactSoundController = contactSoundController;
            this.maxCollisionPower = maxCollisionPower;
            this.minCollisionPower = minCollisionPower;
            this.rigidbody = rigidbody;
            this.halfLength = halfLength;
        }

        private void OnCollisionEnter(Collision collision)
        {
            bool isPlaying = this.contactSoundController.Source.isPlaying;
            if (!isPlaying || (this.contactSoundController.MaxVolume < 0.1f))
            {
                ContactPoint[] contacts = collision.contacts;
                int length = contacts.Length;
                length = (length <= 2) ? length : 2;
                Vector3 zero = Vector3.zero;
                Vector3 nrm = Vector3.zero;
                float num2 = 0f;
                bool flag2 = false;
                bool flag3 = false;
                Vector3 vector3 = this.previousAngularVelocity * this.halfLength;
                Vector3 velocity = this.previousVelocity + vector3;
                Vector3 to = -velocity.normalized;
                for (int i = 0; i < length; i++)
                {
                    RaycastHit hit;
                    ContactPoint point = contacts[i];
                    Collider otherCollider = point.otherCollider;
                    GameObject gameObject = otherCollider.gameObject;
                    int layer = gameObject.layer;
                    Vector3 vector6 = point.point;
                    Vector3 position = base.transform.position;
                    Vector3 vector8 = vector6 - position;
                    float maxDistance = vector8.magnitude + 0.25f;
                    Vector3 normalized = vector8.normalized;
                    Ray ray = new Ray(position, normalized);
                    if (otherCollider.Raycast(ray, out hit, maxDistance) && (hit.collider == otherCollider))
                    {
                        Vector3 normal = hit.normal;
                        if (layer == Layers.TANK_TO_TANK)
                        {
                            float num6 = Mathf.Abs(Vector3.Angle(normal, to));
                            if (!flag2)
                            {
                                flag2 = true;
                                num2 = num6;
                                nrm = normal;
                            }
                            else if (num6 < num2)
                            {
                                num2 = num6;
                                nrm = normal;
                            }
                        }
                        if ((!flag2 && ((layer == Layers.STATIC) && !gameObject.CompareTag(ClientGraphicsConstants.TERRAIN_TAG))) && (Mathf.Abs(Vector3.Angle(normal, Vector3.up)) >= 45f))
                        {
                            float num8 = Mathf.Abs(Vector3.Angle(normal, to));
                            if (!flag3)
                            {
                                flag3 = true;
                                num2 = num8;
                                zero = normal;
                            }
                            else if (num8 < num2)
                            {
                                num2 = num8;
                                zero = normal;
                            }
                        }
                    }
                }
                if (flag2)
                {
                    this.PlayTankCollideSound(velocity, nrm, isPlaying);
                }
                else if (flag3)
                {
                    this.PlayTankCollideSound(velocity, zero, isPlaying);
                }
            }
        }

        private void OnEnable()
        {
            this.velocity = this.previousVelocity = Vector3.zero;
            this.previousAngularVelocity = this.angularVelocity = Vector3.zero;
        }

        private void PlayTankCollideSound(Vector3 velocity, Vector3 nrm, bool isPlaying)
        {
            Vector3 vector = velocity - Vector3.ProjectOnPlane(velocity, nrm);
            float num3 = this.CalculateVolumeByVelocity(vector, this.minCollisionPower, this.maxCollisionPower);
            if ((num3 > 0f) && (!isPlaying || (num3 > this.contactSoundController.MaxVolume)))
            {
                this.contactSoundController.StopImmediately();
                this.contactSoundController.MaxVolume = num3;
                this.contactSoundController.SetSoundActive();
            }
        }
    }
}

