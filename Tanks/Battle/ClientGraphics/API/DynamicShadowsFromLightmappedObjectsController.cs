namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class DynamicShadowsFromLightmappedObjectsController : MonoBehaviour
    {
        public void Awake()
        {
            GameObject shadowCastersRoot = this.CreateShadowCastersRoot();
            foreach (MeshRenderer renderer in base.gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                if (renderer.lightmapIndex >= 0)
                {
                    GameObject obj3 = CreateShadowCaster(renderer, shadowCastersRoot);
                    obj3.AddComponent<MeshFilter>().sharedMesh = renderer.GetComponent<MeshFilter>().sharedMesh;
                    MeshRenderer renderer2 = obj3.AddComponent<MeshRenderer>();
                    renderer2.sharedMaterials = renderer.sharedMaterials;
                    renderer2.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                    renderer2.receiveShadows = false;
                    renderer2.reflectionProbeUsage = ReflectionProbeUsage.Off;
                    renderer2.lightProbeUsage = LightProbeUsage.Off;
                }
            }
        }

        private static GameObject CreateShadowCaster(MeshRenderer meshRenderer, GameObject shadowCastersRoot)
        {
            GameObject obj2 = new GameObject(meshRenderer.name + "_ShadowCaster");
            obj2.transform.SetParent(shadowCastersRoot.transform);
            obj2.transform.position = meshRenderer.transform.position;
            obj2.transform.rotation = meshRenderer.transform.rotation;
            obj2.transform.localScale = meshRenderer.transform.lossyScale;
            return obj2;
        }

        private GameObject CreateShadowCastersRoot()
        {
            GameObject obj2 = new GameObject("DynamicShadowsCasters");
            obj2.transform.SetParent(base.transform);
            obj2.transform.position = Vector3.zero;
            obj2.transform.rotation = Quaternion.identity;
            obj2.transform.localScale = Vector3.one;
            return obj2;
        }
    }
}

