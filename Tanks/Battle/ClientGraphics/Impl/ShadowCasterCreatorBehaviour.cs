namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class ShadowCasterCreatorBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            HashSet<MeshRenderer> set = new HashSet<MeshRenderer>();
            GameObject[] objArray = GameObject.FindGameObjectsWithTag("CASTSHADOW");
            int index = 0;
            while (index < objArray.Length)
            {
                MeshRenderer[] componentsInChildren = objArray[index].GetComponentsInChildren<MeshRenderer>();
                int num2 = 0;
                while (true)
                {
                    if (num2 >= componentsInChildren.Length)
                    {
                        index++;
                        break;
                    }
                    MeshRenderer item = componentsInChildren[num2];
                    set.Add(item);
                    num2++;
                }
            }
            foreach (MeshRenderer renderer2 in set)
            {
                MeshFilter component = renderer2.GetComponent<MeshFilter>();
                if (renderer2.enabled && component)
                {
                    Mesh sharedMesh = component.sharedMesh;
                    GameObject obj3 = new GameObject("Shadow");
                    obj3.AddComponent<MeshFilter>().sharedMesh = sharedMesh;
                    MeshRenderer renderer3 = obj3.AddComponent<MeshRenderer>();
                    renderer3.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                    renderer3.sharedMaterials = renderer2.sharedMaterials;
                    obj3.transform.SetParent(renderer2.transform, false);
                }
            }
        }
    }
}

