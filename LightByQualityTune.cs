using System;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightByQualityTune : MonoBehaviour
{
    public float lowQualityIntensity = 1f;
    public Color lowQualityColor = new Color(1f, 1f, 1f, 1f);
    public int lowQualityLevel = 1;
    public int disableQualityLevel = -1;

    private void Start()
    {
        int qualityLevel = QualitySettings.GetQualityLevel();
        if (qualityLevel <= this.disableQualityLevel)
        {
            base.gameObject.SetActive(false);
        }
        if (qualityLevel <= this.lowQualityLevel)
        {
            Light component = base.GetComponent<Light>();
            component.intensity = this.lowQualityIntensity;
            component.color = this.lowQualityColor;
        }
    }
}

