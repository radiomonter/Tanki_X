using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteInEditMode]
public class ME_ParticleCollisionDecal : MonoBehaviour
{
    public ParticleSystem DecalParticles;
    public bool IsBilboard;
    public bool InstantiateWhenZeroSpeed;
    public float MaxGroundAngleDeviation = 45f;
    public float MinDistanceBetweenDecals = 0.1f;
    public float MinDistanceBetweenSurface = 0.03f;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem.Particle[] particles;
    private ParticleSystem initiatorPS;
    private List<GameObject> collidedGameObjects = new List<GameObject>();

    private void CollisionDetect()
    {
        int aliveParticles = 0;
        if (this.InstantiateWhenZeroSpeed)
        {
            aliveParticles = this.DecalParticles.GetParticles(this.particles);
        }
        foreach (GameObject obj2 in this.collidedGameObjects)
        {
            this.OnParticleCollisionManual(obj2, aliveParticles);
        }
    }

    private void OnDisable()
    {
        if (this.InstantiateWhenZeroSpeed)
        {
            base.CancelInvoke("CollisionDetect");
        }
    }

    private void OnEnable()
    {
        this.collisionEvents.Clear();
        this.collidedGameObjects.Clear();
        this.initiatorPS = base.GetComponent<ParticleSystem>();
        this.particles = new ParticleSystem.Particle[this.DecalParticles.main.maxParticles];
        if (this.InstantiateWhenZeroSpeed)
        {
            base.InvokeRepeating("CollisionDetect", 0f, 0.1f);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!this.InstantiateWhenZeroSpeed)
        {
            this.OnParticleCollisionManual(other, -1);
        }
        else if (!this.collidedGameObjects.Contains(other))
        {
            this.collidedGameObjects.Add(other);
        }
    }

    private void OnParticleCollisionManual(GameObject other, int aliveParticles = -1)
    {
        this.collisionEvents.Clear();
        int num = this.initiatorPS.GetCollisionEvents(other, this.collisionEvents);
        int num2 = 0;
        while (true)
        {
            while (true)
            {
                if (num2 >= num)
                {
                    return;
                }
                ParticleCollisionEvent event2 = this.collisionEvents[num2];
                float num3 = Vector3.Angle(event2.normal, Vector3.up);
                if (num3 <= this.MaxGroundAngleDeviation)
                {
                    if (this.InstantiateWhenZeroSpeed)
                    {
                        if (this.collisionEvents[num2].velocity.sqrMagnitude > 0.1f)
                        {
                            break;
                        }
                        bool flag = false;
                        int index = 0;
                        while (true)
                        {
                            if (index < aliveParticles)
                            {
                                ParticleCollisionEvent event4 = this.collisionEvents[num2];
                                float num5 = Vector3.Distance(event4.intersection, this.particles[index].position);
                                if (num5 < this.MinDistanceBetweenDecals)
                                {
                                    flag = true;
                                }
                                index++;
                                continue;
                            }
                            if (!flag)
                            {
                                break;
                            }
                            break;
                        }
                    }
                    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams {
                        position = this.collisionEvents[num2].intersection + (this.collisionEvents[num2].normal * this.MinDistanceBetweenSurface)
                    };
                    Vector3 eulerAngles = Quaternion.LookRotation(-this.collisionEvents[num2].normal).eulerAngles;
                    eulerAngles.z = Random.Range(0, 360);
                    emitParams.rotation3D = eulerAngles;
                    this.DecalParticles.Emit(emitParams, 1);
                }
                break;
            }
            num2++;
        }
    }
}

