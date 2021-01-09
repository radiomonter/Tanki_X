namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Library.ClientLogger.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class GrassGenerator : MonoBehaviour
    {
        public GrassLocationParams grassLocationParams = new GrassLocationParams();
        public List<GrassPrefabData> grassPrefabDataList = new List<GrassPrefabData>();
        public List<GrassCell> grassCells;
        public float farCullingDistance = 200f;
        public float nearCullingDistance = 100f;
        public float fadeRange = 15f;
        public float denstyMultipler = 1f;

        private void AdjustGrassDensityByVideoMemorySize()
        {
            if (SystemInfo.graphicsMemorySize <= 0x200)
            {
                this.farCullingDistance = 0f;
                this.denstyMultipler = 0f;
            }
            else if (SystemInfo.graphicsMemorySize <= 0x44c)
            {
                this.denstyMultipler = Mathf.Min(0.3f, this.denstyMultipler);
            }
            else if (SystemInfo.graphicsMemorySize <= 0x834)
            {
                this.denstyMultipler = Mathf.Min(0.4f, this.denstyMultipler);
            }
        }

        public void CleanGrassPositions()
        {
            if (this.grassCells != null)
            {
                this.grassCells.Clear();
            }
        }

        private void CombineGrass(MeshBuilder builder, MeshBuffersCache cache, GrassInstancePrototypes grassInstancePrototypes, List<GrassPosition> combinedPositions, GrassColors colors)
        {
            builder.Clear();
            Dictionary<int, float> dictionary = new Dictionary<int, float>();
            int num = 0;
            while (num < combinedPositions.Count)
            {
                GrassPrefabData data;
                Mesh mesh;
                grassInstancePrototypes.GetRandomPrototype(out mesh, out data);
                int[] triangles = cache.GetTriangles(mesh);
                Vector3[] vertices = cache.GetVertices(mesh);
                Vector3[] normals = cache.GetNormals(mesh);
                Vector2[] uVs = cache.GetUVs(mesh);
                GrassPosition position = combinedPositions[num];
                Matrix4x4 randomTransform = this.GetRandomTransform(data, position.position);
                builder.ClearWeldHashing();
                Color white = Color.white;
                if (colors != null)
                {
                    white = colors.GetRandomColor();
                }
                dictionary.Clear();
                int index = 0;
                while (true)
                {
                    if (index >= triangles.Length)
                    {
                        num++;
                        break;
                    }
                    int num3 = triangles[index];
                    int num4 = triangles[index + 1];
                    int num5 = triangles[index + 2];
                    Vector3 v = vertices[num3];
                    Vector3 vector2 = vertices[num4];
                    Vector3 vector3 = vertices[num5];
                    Vector3 vector4 = normals[num3];
                    Vector3 vector5 = normals[num4];
                    Vector3 vector6 = normals[num5];
                    float num6 = 0f;
                    int key = (((int) (vector4.x * 10f)) ^ ((int) (vector4.y * 10f))) ^ ((int) (vector4.z * 10f));
                    if (!dictionary.TryGetValue(key, out num6))
                    {
                        num6 = Random.value;
                        dictionary.Add(key, num6);
                    }
                    if (num6 <= this.denstyMultipler)
                    {
                        v = randomTransform.MultiplyPoint(v);
                        vector2 = randomTransform.MultiplyPoint(vector2);
                        vector3 = randomTransform.MultiplyPoint(vector3);
                        Vector2 lightmapUV = combinedPositions[num].lightmapUV;
                        num3 = builder.AddVertexWeld((long) num3, new VertexData(v, randomTransform.MultiplyVector(vector4), SurfaceType.UNDEFINED), uVs[num3], lightmapUV, white);
                        builder.AddTriangle(num3, builder.AddVertexWeld((long) num4, new VertexData(vector2, randomTransform.MultiplyVector(vector5), SurfaceType.UNDEFINED), uVs[num4], lightmapUV, white), builder.AddVertexWeld((long) num5, new VertexData(vector3, randomTransform.MultiplyVector(vector6), SurfaceType.UNDEFINED), uVs[num5], lightmapUV, white));
                    }
                    index += 3;
                }
            }
        }

        public void DeleteGrass()
        {
            Transform transform = base.gameObject.transform;
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        public void Generate()
        {
            this.DeleteGrass();
            if (this.grassCells.Count == 0)
            {
                LoggerProvider.GetLogger(this).Error($"GrassGenerator {base.name} : combined grass positions are not ready.");
            }
            else
            {
                for (int i = this.grassPrefabDataList.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        this.ValidateGrassPrefab(this.grassPrefabDataList[i]);
                    }
                    catch (Exception exception)
                    {
                        LoggerProvider.GetLogger(this).Error(exception.Message);
                        this.grassPrefabDataList.RemoveAt(i);
                    }
                }
                GrassInstancePrototypes grassInstancePrototypes = new GrassInstancePrototypes();
                try
                {
                    grassInstancePrototypes.CreatePrototypes(this.grassPrefabDataList);
                    this.Generate(grassInstancePrototypes, this.grassCells);
                }
                catch (Exception exception2)
                {
                    LoggerProvider.GetLogger(this).Error("GrassGenerator " + base.name + ": grass generation failed", exception2);
                    this.DeleteGrass();
                }
                finally
                {
                    grassInstancePrototypes.DestroyPrototypes();
                }
            }
        }

        private void Generate(GrassInstancePrototypes grassInstancePrototypes, List<GrassCell> grassCells)
        {
            MeshRenderer component = this.grassPrefabDataList[0].grassPrefab.GetComponent<MeshRenderer>();
            MeshBuilder builder = new MeshBuilder();
            MeshBuffersCache cache = new MeshBuffersCache();
            GrassColors componentInParent = base.GetComponentInParent<GrassColors>();
            Dictionary<int, Material> dictionary = new Dictionary<int, Material>();
            foreach (GrassCell cell in grassCells)
            {
                Material material;
                if (!dictionary.TryGetValue(cell.lightmapId, out material))
                {
                    material = new Material(component.sharedMaterial);
                    material.SetFloat("_GrassCullingRange", this.fadeRange);
                    material.SetFloat("_GrassCullingDistance", this.farCullingDistance);
                    if (cell.lightmapId >= 0)
                    {
                        material.SetTexture("_LightMap", LightmapSettings.lightmaps[cell.lightmapId].lightmapColor);
                    }
                    dictionary.Add(cell.lightmapId, material);
                }
                this.CombineGrass(builder, cache, grassInstancePrototypes, cell.grassPositions, componentInParent);
                Mesh mesh = new Mesh();
                builder.BuildToMesh(mesh, false);
                GameObject obj2 = new GameObject("GrassCell") {
                    layer = Layers.GRASS
                };
                obj2.AddComponent<MeshFilter>().sharedMesh = mesh;
                MeshRenderer renderer2 = obj2.AddComponent<MeshRenderer>();
                renderer2.material = material;
                renderer2.receiveShadows = component.receiveShadows;
                renderer2.shadowCastingMode = component.shadowCastingMode;
                renderer2.lightProbeUsage = LightProbeUsage.Off;
                obj2.transform.SetParent(base.gameObject.transform, true);
                obj2.transform.position = cell.center;
                obj2.isStatic = true;
            }
        }

        private Quaternion GetRandomRotation()
        {
            Quaternion identity = Quaternion.identity;
            identity.eulerAngles = new Vector3(0f, Random.value * 360f, 0f);
            return identity;
        }

        private Matrix4x4 GetRandomTransform(GrassPrefabData grassPrefabData, Vector3 position)
        {
            Quaternion q = this.GetRandomRotation() * grassPrefabData.grassPrefab.transform.rotation;
            float minScale = grassPrefabData.minScale;
            float x = minScale + (Random.value * (grassPrefabData.maxScale - minScale));
            return Matrix4x4.TRS(position, q, new Vector3(x, x, x));
        }

        private bool GrassPrefabsAreValid()
        {
            try
            {
                this.ValidateGrassPrefabs();
                return true;
            }
            catch (GrassGeneratorException)
            {
                return false;
            }
        }

        public bool HasGeneratedGrass() => 
            base.gameObject.transform.childCount > 0;

        private void InitCameraCulling()
        {
            Camera main = Camera.main;
            float[] layerCullDistances = main.layerCullDistances;
            layerCullDistances[Layers.GRASS] = this.farCullingDistance;
            main.layerCullDistances = layerCullDistances;
        }

        public void SetCulling(float farCullingDistance, float nearCullingDistance, float fadeRange, float denstyMultipler)
        {
            this.farCullingDistance = farCullingDistance;
            this.nearCullingDistance = nearCullingDistance;
            this.fadeRange = fadeRange;
            this.denstyMultipler = denstyMultipler;
            this.InitCameraCulling();
            this.AdjustGrassDensityByVideoMemorySize();
        }

        public void Validate()
        {
            this.ValidateGrassLocation();
            this.ValidateGrassPrefabs();
        }

        private void ValidateGrassLocation()
        {
            if (this.grassLocationParams.uvMask == null)
            {
                throw new GrassGeneratorException($"GrassGenerator {base.name}: <b>Mask</b> isn't set");
            }
            if (this.grassLocationParams.terrainObjects.Count == 0)
            {
                throw new GrassGeneratorException($"GrassGenerator {base.name}: <b>Terrain objects</b> aren't set");
            }
            for (int i = 0; i < this.grassLocationParams.terrainObjects.Count; i++)
            {
                GameObject obj2 = this.grassLocationParams.terrainObjects[i];
                if (obj2 == null)
                {
                    throw new GrassGeneratorException($"GrassGenerator {base.name}: terrainObject '{i}' isn't set");
                }
            }
            if (this.grassLocationParams.densityPerMeter <= 0f)
            {
                throw new GrassGeneratorException($"GrassGenerator {base.name}: <b>Density</b> {this.grassLocationParams.densityPerMeter} is incorrect. Density has to be more than zero.");
            }
            if (this.grassLocationParams.grassCombineWidth <= 0f)
            {
                throw new GrassGeneratorException($"GrassGenerator {base.name}: <b>Grass combine width</b> {this.grassLocationParams.grassCombineWidth} is incorrect.It has to be more than zero");
            }
        }

        private void ValidateGrassPrefab(GrassPrefabData grassPrefabData)
        {
            string str;
            if (!grassPrefabData.IsValid(out str))
            {
                throw new GrassGeneratorException($"GrassGenerator {base.name}: grass prefab {grassPrefabData} is not valid. {str} ");
            }
        }

        public void ValidateGrassPrefabs()
        {
            if (this.grassPrefabDataList.Count == 0)
            {
                throw new GrassGeneratorException($"GrassGenerator {base.name}: List of grass prefabs is empty");
            }
            for (int i = this.grassPrefabDataList.Count - 1; i >= 0; i--)
            {
                this.ValidateGrassPrefab(this.grassPrefabDataList[i]);
            }
        }

        public bool ReadyForGeneration =>
            ((this.grassCells != null) && (this.grassCells.Count > 0)) && this.GrassPrefabsAreValid();
    }
}

