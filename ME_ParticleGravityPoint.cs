using System;
using UnityEngine;

[ExecuteInEditMode]
public class ME_ParticleGravityPoint : MonoBehaviour
{
    public Transform target;
    public float Force = 1f;
    public bool DistanceRelative;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private ParticleSystem.MainModule mainModule;
    private Vector3 prevPos;

    private unsafe void LateUpdate()
    {
        int maxParticles = this.mainModule.maxParticles;
        if ((this.particles == null) || (this.particles.Length < maxParticles))
        {
            this.particles = new ParticleSystem.Particle[maxParticles];
        }
        int particles = this.ps.GetParticles(this.particles);
        Vector3 zero = Vector3.zero;
        if (this.mainModule.simulationSpace == ParticleSystemSimulationSpace.Local)
        {
            zero = base.transform.InverseTransformPoint(this.target.position);
        }
        if (this.mainModule.simulationSpace == ParticleSystemSimulationSpace.World)
        {
            zero = this.target.position;
        }
        float num3 = Time.deltaTime * this.Force;
        if (this.DistanceRelative)
        {
            num3 *= Mathf.Abs((this.prevPos - zero).magnitude);
        }
        for (int i = 0; i < particles; i++)
        {
            Vector3 vector3 = zero - this.particles[i].position;
            Vector3 vector4 = Vector3.Normalize(vector3);
            if (this.DistanceRelative)
            {
                vector4 = Vector3.Normalize(zero - this.prevPos);
            }
            Vector3 vector5 = vector4 * num3;
            ParticleSystem.Particle* particlePtr1 = &(this.particles[i]);
            particlePtr1.velocity += vector5;
        }
        this.ps.SetParticles(this.particles, particles);
        this.prevPos = zero;
    }

    private void Start()
    {
        this.ps = base.GetComponent<ParticleSystem>();
        this.mainModule = this.ps.main;
    }
}

