namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class TutorialTankPartOutline : MonoBehaviour
    {
        public Shader outlineShader;

        public void Disable()
        {
            OutlineObject component = base.GetComponent<OutlineObject>();
            if (component != null)
            {
                component.Enable = false;
            }
        }

        public void Init(GameObject tankPart)
        {
            SkinnedMeshRenderer component = tankPart.GetComponent<SkinnedMeshRenderer>();
            Material material = new Material(this.outlineShader);
            base.gameObject.AddComponent<MeshFilter>().sharedMesh = component.sharedMesh;
            MeshRenderer renderer2 = base.gameObject.AddComponent<MeshRenderer>();
            Material[] materialArray = new Material[component.sharedMaterials.Length];
            for (int i = 0; i < materialArray.Length; i++)
            {
                materialArray[i] = material;
            }
            renderer2.sharedMaterials = materialArray;
            renderer2.lightProbeUsage = LightProbeUsage.Off;
            renderer2.reflectionProbeUsage = ReflectionProbeUsage.Off;
            renderer2.shadowCastingMode = ShadowCastingMode.Off;
            renderer2.receiveShadows = false;
            if ((base.transform.parent != null) && (base.transform.parent.GetComponent<TutorialTankPartOutline>() != null))
            {
                Destroy(base.gameObject.GetComponent<OutlineObject>());
            }
        }
    }
}

