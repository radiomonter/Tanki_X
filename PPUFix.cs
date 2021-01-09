using System;
using UnityEngine;

public class PPUFix : MonoBehaviour
{
    private Canvas canvas;
    private float prevPPU;

    private void Start()
    {
        this.canvas = base.GetComponent<Canvas>();
    }

    private void Update()
    {
        float a = 100f / this.canvas.scaleFactor;
        if (!Mathf.Approximately(a, this.prevPPU))
        {
            this.prevPPU = a;
            this.canvas.referencePixelsPerUnit = a;
        }
    }
}

