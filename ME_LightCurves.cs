using System;
using UnityEngine;

public class ME_LightCurves : MonoBehaviour
{
    public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float GraphTimeMultiplier = 1f;
    public float GraphIntensityMultiplier = 1f;
    public bool IsLoop;
    private bool canUpdate;
    private float startTime;
    private Light lightSource;

    private void Awake()
    {
        this.lightSource = base.GetComponent<Light>();
        this.lightSource.intensity = this.LightCurve.Evaluate(0f);
    }

    private void OnEnable()
    {
        this.startTime = Time.time;
        this.canUpdate = true;
    }

    private void Update()
    {
        float num = Time.time - this.startTime;
        if (this.canUpdate)
        {
            float num2 = this.LightCurve.Evaluate(num / this.GraphTimeMultiplier) * this.GraphIntensityMultiplier;
            this.lightSource.intensity = num2;
        }
        if (num >= this.GraphTimeMultiplier)
        {
            if (this.IsLoop)
            {
                this.startTime = Time.time;
            }
            else
            {
                this.canUpdate = false;
            }
        }
    }
}

