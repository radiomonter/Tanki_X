using System;
using UnityEngine;

public class PostEffectsSet : MonoBehaviour
{
    public string qualityName;
    public DepthTextureMode depthTextureMode;
    [SerializeField]
    private MonoBehaviour[] effects;

    public void SetActive(bool value)
    {
        if (this.effects != null)
        {
            for (int i = 0; i < this.effects.Length; i++)
            {
                this.effects[i].enabled = value;
            }
        }
        base.enabled = value;
    }
}

