namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientGraphics.Impl.Batching.Zones;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class ZoneBatching : MonoBehaviour
    {
        public float size = 10f;
        public bool castShadows = true;
        public bool receiveShadows = true;
        public bool useLigthProbes;
        public Transform anchorOverride;
        public bool encodePositionInColor;
        private List<Zone> zones;
        [NonSerialized]
        public int beforeSubmeshes;
        [NonSerialized]
        public int afterSubmeshes;
        private GroupingOrderComparer orderComparer = new GroupingOrderComparer();
        private CandidatesComparer candidatesComparer = new CandidatesComparer();
        private List<Submesh> candidates = new List<Submesh>();

        private static Bounds CalculatePartBounds(Vector3[] vertices, int[] triangles, Matrix4x4 matrix)
        {
            bool flag = false;
            Bounds bounds = new Bounds();
            for (int i = 0; i < triangles.Length; i++)
            {
                int index = triangles[i];
                Vector3 point = matrix.MultiplyPoint3x4(vertices[index]);
                if (flag)
                {
                    bounds.Encapsulate(point);
                }
                else
                {
                    bounds.center = point;
                    flag = true;
                }
            }
            return bounds;
        }

        private static bool CanGroup(Bounds a, Bounds b, float maxSize)
        {
            float magnitude = (a.center - b.center).magnitude;
            return (((a.extents.magnitude + b.extents.magnitude) + magnitude) <= maxSize);
        }

        private void CollectMeshParts(List<Submesh> collector, Mesh mesh, MeshRenderer renderer, float maxSize)
        {
            Matrix4x4 localToWorldMatrix = renderer.localToWorldMatrix;
            Material[] sharedMaterials = renderer.sharedMaterials;
            int index = 0;
            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                if (index >= sharedMaterials.Length)
                {
                    index = sharedMaterials.Length - 1;
                }
                Bounds bounds = CalculatePartBounds(vertices, mesh.GetTriangles(i), localToWorldMatrix);
                if ((2f * bounds.extents.magnitude) <= maxSize)
                {
                    Submesh item = new Submesh {
                        renderer = renderer,
                        material = sharedMaterials[index],
                        mesh = mesh,
                        submesh = i,
                        bounds = bounds
                    };
                    collector.Add(item);
                }
                index++;
            }
        }

        private List<Submesh> CollectParts(GameObject root, float maxSize)
        {
            MeshRenderer[] componentsInChildren = root.GetComponentsInChildren<MeshRenderer>();
            List<Submesh> collector = new List<Submesh>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                MeshRenderer renderer = componentsInChildren[i];
                if (renderer.enabled && !renderer.isPartOfStaticBatch)
                {
                    MeshFilter component = renderer.GetComponent<MeshFilter>();
                    Mesh sharedMesh = component?.sharedMesh;
                    if (sharedMesh != null)
                    {
                        this.beforeSubmeshes += sharedMesh.subMeshCount;
                        this.CollectMeshParts(collector, sharedMesh, renderer, maxSize);
                    }
                }
            }
            return collector;
        }

        private Mesh CombineMeshes(List<Submesh> meshes, Bounds bounds, bool moveInCenter)
        {
            Mesh mesh = new Mesh();
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            int num = 0;
            List<Vector3> list = new List<Vector3>();
            List<Vector2> list2 = new List<Vector2>();
            List<Vector2> list3 = new List<Vector2>();
            List<Vector3> list4 = new List<Vector3>();
            List<Vector4> list5 = new List<Vector4>();
            List<int> list6 = new List<int>();
            List<Color32> list7 = null;
            if (this.encodePositionInColor)
            {
                list7 = new List<Color32>();
            }
            Vector2 defaultValue = new Vector2(0f, 0f);
            Vector3 vector2 = new Vector3(0f, 0f, 1f);
            Vector4 vector3 = new Vector4(1f, 0f, 0f, 1f);
            Vector3 center = bounds.center;
            for (int i = 0; i < meshes.Count; i++)
            {
                Submesh submesh = meshes[i];
                Mesh mesh2 = submesh.mesh;
                int num3 = submesh.submesh;
                Matrix4x4 localToWorldMatrix = submesh.renderer.localToWorldMatrix;
                Matrix4x4 worldToLocalMatrix = submesh.renderer.worldToLocalMatrix;
                Vector4 lightmapScaleOffset = submesh.renderer.lightmapScaleOffset;
                dictionary.Clear();
                int[] triangles = mesh2.GetTriangles(num3);
                if (triangles.Length != 0)
                {
                    Vector3[] vertices = mesh2.vertices;
                    Vector2[] uv = mesh2.uv;
                    Vector2[] vectorArray3 = (mesh2.uv2.Length <= 0) ? null : mesh2.uv2;
                    Vector3[] vectorArray4 = (mesh2.normals.Length <= 0) ? null : mesh2.normals;
                    Vector4[] vectorArray5 = (mesh2.tangents.Length <= 0) ? null : mesh2.tangents;
                    for (int j = 0; j < triangles.Length; j++)
                    {
                        int num6;
                        int key = triangles[j];
                        if (dictionary.ContainsKey(key))
                        {
                            num6 = dictionary[key];
                        }
                        else
                        {
                            num6 = num;
                            dictionary.Add(key, num6);
                            num++;
                            Vector3 item = localToWorldMatrix.MultiplyPoint3x4(vertices[key]);
                            if (moveInCenter)
                            {
                                item -= center;
                            }
                            list.Add(item);
                            list2.Add(uv[key]);
                            if (vectorArray3 != null)
                            {
                                UpdateListMinCount<Vector2>(list3, num6, defaultValue);
                                Vector2 vector7 = vectorArray3[key];
                                list3.Add(new Vector2((lightmapScaleOffset.x * vector7.x) + lightmapScaleOffset.z, (lightmapScaleOffset.y * vector7.y) + lightmapScaleOffset.w));
                            }
                            if (vectorArray5 != null)
                            {
                                UpdateListMinCount<Vector4>(list5, num6, vector3);
                                Vector4 vector8 = vectorArray5[key];
                                float num9 = ((localToWorldMatrix[0, 0] * vector8.x) + (localToWorldMatrix[0, 1] * vector8.y)) + (localToWorldMatrix[0, 2] * vector8.z);
                                float num10 = ((localToWorldMatrix[1, 0] * vector8.x) + (localToWorldMatrix[1, 1] * vector8.y)) + (localToWorldMatrix[1, 2] * vector8.z);
                                float num11 = ((localToWorldMatrix[2, 0] * vector8.x) + (localToWorldMatrix[2, 1] * vector8.y)) + (localToWorldMatrix[2, 2] * vector8.z);
                                float num12 = Mathf.Sqrt(((num9 * num9) + (num10 * num10)) + (num11 * num11));
                                list5.Add((num12 >= 1E-06) ? new Vector4(num9 / num12, num10 / num12, num11 / num12, vector8.w) : new Vector4(1f, 0f, 0f, vector8.w));
                            }
                            if (vectorArray4 != null)
                            {
                                UpdateListMinCount<Vector3>(list4, num6, vector2);
                                Vector3 vector10 = vectorArray4[key];
                                float num13 = ((vector10.x * worldToLocalMatrix[0, 0]) + (vector10.y * worldToLocalMatrix[1, 0])) + (vector10.z * worldToLocalMatrix[2, 0]);
                                float num14 = ((vector10.x * worldToLocalMatrix[0, 1]) + (vector10.y * worldToLocalMatrix[1, 1])) + (vector10.z * worldToLocalMatrix[2, 1]);
                                float num15 = ((vector10.x * worldToLocalMatrix[0, 2]) + (vector10.y * worldToLocalMatrix[1, 2])) + (vector10.z * worldToLocalMatrix[2, 2]);
                                float num16 = Mathf.Sqrt(((num13 * num13) + (num14 * num14)) + (num15 * num15));
                                list4.Add((num16 >= 1E-06) ? new Vector3(num13 / num16, num14 / num16, num15 / num16) : vector2);
                            }
                            if (this.encodePositionInColor)
                            {
                                Vector4 column = localToWorldMatrix.GetColumn(3);
                                Color32 color = new Color32((byte) ((column.x % 1f) * 256f), (byte) ((column.y % 1f) * 256f), (byte) ((column.z % 1f) * 256f), 0xff);
                                list7.Add(color);
                            }
                        }
                        list6.Add(num6);
                    }
                }
            }
            mesh.vertices = list.ToArray();
            mesh.uv = list2.ToArray();
            mesh.uv2 = list3.ToArray();
            mesh.normals = list4.ToArray();
            mesh.tangents = list5.ToArray();
            mesh.triangles = list6.ToArray();
            if (this.encodePositionInColor)
            {
                mesh.colors32 = list7.ToArray();
            }
            bounds.center = Vector3.zero;
            mesh.bounds = bounds;
            return mesh;
        }

        private void CombineParts(List<Zone> zones)
        {
            for (int i = 0; i < zones.Count; i++)
            {
                Zone zone = zones[i];
                Mesh mesh = this.CombineMeshes(zone.contents, zone.bounds, true);
                string name = "Zone " + zone.material.name;
                if (zone.lightmapIndex >= 0)
                {
                    name = name + " " + zone.lightmapIndex;
                }
                GameObject obj2 = new GameObject(name) {
                    layer = base.gameObject.layer
                };
                obj2.AddComponent<MeshFilter>().sharedMesh = mesh;
                MeshRenderer renderer = obj2.AddComponent<MeshRenderer>();
                renderer.material = zone.material;
                renderer.lightmapIndex = zone.lightmapIndex;
                renderer.shadowCastingMode = !this.castShadows ? ShadowCastingMode.Off : ShadowCastingMode.On;
                renderer.receiveShadows = this.receiveShadows;
                renderer.useLightProbes = this.useLigthProbes;
                if (this.anchorOverride != null)
                {
                    renderer.probeAnchor = this.anchorOverride;
                }
                obj2.transform.position = zone.bounds.center;
                obj2.transform.parent = base.transform;
            }
        }

        private List<Zone> CombineZones(List<Submesh> parts, float maxSize)
        {
            this.SortForBetterGrouping(parts, maxSize);
            List<Zone> list = new List<Zone>();
            for (int i = 0; i < parts.Count; i++)
            {
                Submesh item = parts[i];
                if (!item.merged && (item.nearValue != 0))
                {
                    Vector3 vector = new Vector3();
                    int num2 = i + 1;
                    while (true)
                    {
                        if (num2 >= parts.Count)
                        {
                            if (this.candidates.Count > 0)
                            {
                                Zone zone = new Zone {
                                    bounds = item.bounds,
                                    contents = new List<Submesh>()
                                };
                                zone.contents.Add(item);
                                item.merged = true;
                                this.candidatesComparer.center = vector / ((float) this.candidates.Count);
                                this.candidates.Sort(this.candidatesComparer);
                                int num3 = 0;
                                while (true)
                                {
                                    if (num3 >= this.candidates.Count)
                                    {
                                        list.Add(zone);
                                        this.candidates.Clear();
                                        break;
                                    }
                                    Submesh submesh3 = this.candidates[num3];
                                    if ((num3 == 0) || CanGroup(zone.bounds, submesh3.bounds, maxSize))
                                    {
                                        zone.bounds.Encapsulate(submesh3.bounds);
                                        zone.contents.Add(submesh3);
                                        submesh3.merged = true;
                                    }
                                    num3++;
                                }
                            }
                            break;
                        }
                        Submesh submesh2 = parts[num2];
                        if (!submesh2.merged && ((submesh2.material == item.material) && ((submesh2.lightmapIndex == item.lightmapIndex) && CanGroup(item.bounds, submesh2.bounds, maxSize))))
                        {
                            vector += submesh2.bounds.center;
                            this.candidates.Add(submesh2);
                        }
                        num2++;
                    }
                }
            }
            return list;
        }

        private static bool ContainsSubmeshWithIndex(List<Submesh> list, int index)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Submesh submesh = list[i];
                if (index == submesh.submesh)
                {
                    return true;
                }
            }
            return false;
        }

        private static void GizmoBounds(Bounds bounds, Color color, Transform transform = null)
        {
            Vector3 position = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
            Vector3 vector5 = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            Vector3 vector9 = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            Vector3 vector13 = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            Vector3 vector17 = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            Vector3 vector21 = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            Vector3 vector25 = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
            Vector3 vector29 = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
            if (transform != null)
            {
                position = transform.TransformPoint(position);
                vector5 = transform.TransformPoint(vector5);
                vector9 = transform.TransformPoint(vector9);
                vector13 = transform.TransformPoint(vector13);
                vector17 = transform.TransformPoint(vector17);
                vector21 = transform.TransformPoint(vector21);
                vector25 = transform.TransformPoint(vector25);
                vector29 = transform.TransformPoint(vector29);
            }
            Gizmos.color = color;
            Gizmos.DrawLine(position, vector5);
            Gizmos.DrawLine(vector5, vector9);
            Gizmos.DrawLine(vector9, vector13);
            Gizmos.DrawLine(vector13, position);
            Gizmos.DrawLine(vector17, vector21);
            Gizmos.DrawLine(vector21, vector25);
            Gizmos.DrawLine(vector25, vector29);
            Gizmos.DrawLine(vector29, vector17);
            Gizmos.DrawLine(position, vector17);
            Gizmos.DrawLine(vector5, vector21);
            Gizmos.DrawLine(vector13, vector29);
            Gizmos.DrawLine(vector9, vector25);
        }

        private static bool HasImportantComponents(GameObject gm)
        {
            foreach (Component component in gm.GetComponents<Component>())
            {
                if (!(component is Transform) && (!(component is MeshFilter) && !(component is MeshRenderer)))
                {
                    return true;
                }
            }
            return false;
        }

        private void MakeBatches()
        {
            this.CombineParts(this.zones);
            this.RemoveOriginalSubmeshes(this.zones);
        }

        private void OnDrawGizmosSelected()
        {
            if (base.enabled)
            {
                for (int i = 0; i < this.zones.Count; i++)
                {
                    GizmoBounds(this.zones[i].bounds, Color.yellow, null);
                }
            }
        }

        private void OnValidate()
        {
            this.SetupZones();
        }

        private void RemoveOriginalSubmeshes(List<Zone> zones)
        {
            Dictionary<MeshRenderer, List<Submesh>> dictionary = new Dictionary<MeshRenderer, List<Submesh>>();
            int num = 0;
            while (num < zones.Count)
            {
                Zone zone = zones[num];
                int num2 = 0;
                while (true)
                {
                    if (num2 >= zone.contents.Count)
                    {
                        num++;
                        break;
                    }
                    Submesh item = zone.contents[num2];
                    MeshRenderer key = item.renderer;
                    List<Submesh> list = null;
                    if (dictionary.ContainsKey(key))
                    {
                        list = dictionary[key];
                    }
                    else
                    {
                        list = new List<Submesh>();
                        dictionary.Add(key, list);
                    }
                    list.Add(item);
                    num2++;
                }
            }
            foreach (KeyValuePair<MeshRenderer, List<Submesh>> pair in dictionary)
            {
                MeshRenderer key = pair.Key;
                List<Submesh> list = pair.Value;
                MeshFilter component = key.GetComponent<MeshFilter>();
                Mesh sharedMesh = component.sharedMesh;
                if (sharedMesh.subMeshCount != list.Count)
                {
                    this.RemoveSubmeshesFromMesh(component, key, list);
                    continue;
                }
                Destroy(component);
                Destroy(key);
                GameObject gameObject = key.gameObject;
                if ((gameObject.transform.childCount == 0) && !HasImportantComponents(gameObject))
                {
                    key.gameObject.SetActive(false);
                }
            }
        }

        private void RemoveSubmeshesFromMesh(MeshFilter meshFilter, MeshRenderer meshRenderer, List<Submesh> list)
        {
            Mesh sharedMesh = meshFilter.sharedMesh;
            Mesh mesh2 = new Mesh();
            Material[] materials = meshRenderer.materials;
            List<Material> list2 = new List<Material>();
            int index = 0;
            List<CombineInstance> list3 = new List<CombineInstance>();
            for (int i = 0; i < sharedMesh.subMeshCount; i++)
            {
                if (index >= materials.Length)
                {
                    index = materials.Length - 1;
                }
                if (!ContainsSubmeshWithIndex(list, i))
                {
                    CombineInstance item = new CombineInstance {
                        mesh = sharedMesh,
                        subMeshIndex = i
                    };
                    list3.Add(item);
                    list2.Add(materials[index]);
                }
                index++;
            }
            mesh2.CombineMeshes(list3.ToArray(), false, false);
            meshFilter.mesh = mesh2;
            meshRenderer.materials = list2.ToArray();
        }

        public void SetupZones()
        {
            this.beforeSubmeshes = 0;
            List<Submesh> parts = this.CollectParts(base.gameObject, this.size);
            List<Zone> list2 = this.CombineZones(parts, this.size);
            this.afterSubmeshes = this.beforeSubmeshes;
            for (int i = 0; i < list2.Count; i++)
            {
                Zone zone = list2[i];
                this.afterSubmeshes = (this.afterSubmeshes - zone.contents.Count) + 1;
            }
            this.zones = (list2.Count <= 0) ? new List<Zone>() : list2;
        }

        private void SortForBetterGrouping(List<Submesh> parts, float maxSize)
        {
            int num = 0;
            while (num < parts.Count)
            {
                Submesh submesh = parts[num];
                int num2 = num + 1;
                while (true)
                {
                    if (num2 >= parts.Count)
                    {
                        num++;
                        break;
                    }
                    Submesh submesh2 = parts[num2];
                    Vector3 extents = submesh.bounds.extents;
                    float magnitude = extents.magnitude;
                    Vector3 vector2 = submesh2.bounds.extents;
                    Vector3 vector3 = submesh.bounds.center - submesh2.bounds.center;
                    float num4 = (vector2.magnitude + vector3.magnitude) + magnitude;
                    if (num4 <= maxSize)
                    {
                        float num5 = ((num4 - magnitude) - magnitude) / magnitude;
                        submesh.nearValue = (num5 > 0.1) ? ((num5 > 0.2) ? ((num5 > 0.3) ? (submesh.nearValue + 1) : (submesh.nearValue + 3)) : (submesh.nearValue + 5)) : (submesh.nearValue + 10);
                    }
                    num2++;
                }
            }
            parts.Sort(this.orderComparer);
        }

        private void Start()
        {
            this.SetupZones();
            this.MakeBatches();
        }

        private static void UpdateListMinCount<T>(List<T> list, int minCount, T defaultValue)
        {
            for (int i = list.Count; i < minCount; i++)
            {
                list.Add(defaultValue);
            }
        }
    }
}

