using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ME_ParticleTrails : MonoBehaviour
{
    public GameObject TrailPrefab;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private Dictionary<uint, GameObject> hashTrails = new Dictionary<uint, GameObject>();
    private Dictionary<uint, GameObject> newHashTrails = new Dictionary<uint, GameObject>();
    private List<GameObject> currentGO = new List<GameObject>();
    [CompilerGenerated]
    private static Func<KeyValuePair<uint, GameObject>, bool> <>f__am$cache0;
    [CompilerGenerated]
    private static Func<KeyValuePair<uint, GameObject>, uint> <>f__am$cache1;
    [CompilerGenerated]
    private static Func<KeyValuePair<uint, GameObject>, GameObject> <>f__am$cache2;

    public void AddRange<T, S>(Dictionary<T, S> source, Dictionary<T, S> collection)
    {
        if (collection != null)
        {
            foreach (KeyValuePair<T, S> pair in collection)
            {
                if (!source.ContainsKey(pair.Key))
                {
                    source.Add(pair.Key, pair.Value);
                }
            }
        }
    }

    public void Clear()
    {
        foreach (GameObject obj2 in this.currentGO)
        {
            Destroy(obj2);
        }
        this.currentGO.Clear();
    }

    private void ClearEmptyHashes()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = h => h.Value != null;
        }
        <>f__am$cache1 ??= h => h.Key;
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = h => h.Value;
        }
        this.hashTrails = this.hashTrails.Where<KeyValuePair<uint, GameObject>>(<>f__am$cache0).ToDictionary<KeyValuePair<uint, GameObject>, uint, GameObject>(<>f__am$cache1, <>f__am$cache2);
    }

    private void OnDisable()
    {
        this.Clear();
        base.CancelInvoke("ClearEmptyHashes");
    }

    private void OnEnable()
    {
        base.InvokeRepeating("ClearEmptyHashes", 1f, 1f);
    }

    private void Start()
    {
        this.ps = base.GetComponent<ParticleSystem>();
        this.particles = new ParticleSystem.Particle[this.ps.main.maxParticles];
    }

    private void Update()
    {
        this.UpdateTrail();
    }

    private void UpdateTrail()
    {
        this.newHashTrails.Clear();
        int particles = this.ps.GetParticles(this.particles);
        for (int i = 0; i < particles; i++)
        {
            if (!this.hashTrails.ContainsKey(this.particles[i].randomSeed))
            {
                LineRenderer renderer;
                Quaternion rotation = new Quaternion();
                GameObject item = Instantiate<GameObject>(this.TrailPrefab, base.transform.position, rotation);
                item.transform.parent = base.transform;
                this.currentGO.Add(item);
                this.newHashTrails.Add(this.particles[i].randomSeed, item);
                item.GetComponent<LineRenderer>().widthMultiplier = renderer.widthMultiplier * this.particles[i].startSize;
            }
            else
            {
                GameObject obj3 = this.hashTrails[this.particles[i].randomSeed];
                if (obj3 != null)
                {
                    LineRenderer component = obj3.GetComponent<LineRenderer>();
                    component.startColor *= this.particles[i].GetCurrentColor(this.ps);
                    component.endColor *= this.particles[i].GetCurrentColor(this.ps);
                    if (this.ps.main.simulationSpace == ParticleSystemSimulationSpace.World)
                    {
                        obj3.transform.position = this.particles[i].position;
                    }
                    if (this.ps.main.simulationSpace == ParticleSystemSimulationSpace.Local)
                    {
                        obj3.transform.position = this.ps.transform.TransformPoint(this.particles[i].position);
                    }
                    this.newHashTrails.Add(this.particles[i].randomSeed, obj3);
                }
                this.hashTrails.Remove(this.particles[i].randomSeed);
            }
        }
        foreach (KeyValuePair<uint, GameObject> pair in this.hashTrails)
        {
            if (pair.Value != null)
            {
                pair.Value.GetComponent<ME_TrailRendererNoise>().IsActive = false;
            }
        }
        this.AddRange<uint, GameObject>(this.hashTrails, this.newHashTrails);
    }
}

