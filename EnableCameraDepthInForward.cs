using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EnableCameraDepthInForward : MonoBehaviour
{
    private void Set()
    {
        base.GetComponent<Camera>().depthTextureMode ??= DepthTextureMode.Depth;
    }

    private void Start()
    {
        this.Set();
    }
}

