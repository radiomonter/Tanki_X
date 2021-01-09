namespace UnityStandardAssets.Water
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode]
    public class WaterBasic : MonoBehaviour
    {
        private void Update()
        {
            Renderer component = base.GetComponent<Renderer>();
            if (component)
            {
                Material sharedMaterial = component.sharedMaterial;
                if (sharedMaterial)
                {
                    Vector4 vector2 = sharedMaterial.GetVector("WaveSpeed") * ((Time.time / 20f) * sharedMaterial.GetFloat("_WaveScale"));
                    Vector4 vector3 = new Vector4(Mathf.Repeat(vector2.x, 1f), Mathf.Repeat(vector2.y, 1f), Mathf.Repeat(vector2.z, 1f), Mathf.Repeat(vector2.w, 1f));
                    sharedMaterial.SetVector("_WaveOffset", vector3);
                }
            }
        }
    }
}

