namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    public abstract class DynamicDecalManager
    {
        private const int DECAL_COUNT_LIMIT = 500;
        protected const float DECAL_FADE_TIME = 2f;
        private const float SHADER_TIME_DIMENSION = 0.0001f;
        protected LinkedList<DecalEntry> decalsQueue;
        private int maxDecalCount;
        protected int decalsCount;
        protected float decalLifeTimeKoeff;
        private GameObject root;

        public DynamicDecalManager(GameObject root, int maxDecalCount, float decalLifeTimeKoeff, LinkedList<DecalEntry> decalsQueue)
        {
            this.root = root;
            this.maxDecalCount = Math.Min(500, maxDecalCount);
            this.decalLifeTimeKoeff = decalLifeTimeKoeff;
            this.decalsQueue = decalsQueue;
        }

        protected DecalEntry CreateDecalEntry(Mesh decalMesh, Material material, float timeToDestroy)
        {
            GameObject obj2 = new GameObject(this.DecalMeshObjectName);
            obj2.AddComponent<MeshFilter>().mesh = decalMesh;
            MeshRenderer renderer = obj2.AddComponent<MeshRenderer>();
            renderer.material = new Material(material);
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = true;
            renderer.useLightProbes = false;
            obj2.transform.parent = this.root.transform;
            obj2.transform.position = Vector3.zero;
            obj2.transform.rotation = Quaternion.identity;
            return new DecalEntry { 
                gameObject = obj2,
                material = renderer.material,
                timeToDestroy = timeToDestroy
            };
        }

        protected void SetMeshColorAndLifeTime(Mesh mesh, Color color, float lifeTime)
        {
            color.a = (Time.timeSinceLevelLoad + lifeTime) * 0.0001f;
            int vertexCount = mesh.vertexCount;
            Color[] colorArray = new Color[mesh.vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                colorArray[i] = color;
            }
            mesh.colors = colorArray;
        }

        protected void TrimQueue()
        {
            if (this.decalsCount > this.maxDecalCount)
            {
                LinkedListNode<DecalEntry> next;
                DecalEntryType decalType = this.DecalType;
                for (LinkedListNode<DecalEntry> node = this.decalsQueue.First; node != null; node = next)
                {
                    next = node.Next;
                    DecalEntry entry = node.Value;
                    if (entry.type == decalType)
                    {
                        Object.Destroy(node.Value.gameObject);
                        this.decalsQueue.Remove(node);
                        this.decalsCount--;
                        break;
                    }
                }
            }
        }

        protected abstract DecalEntryType DecalType { get; }

        protected abstract string DecalMeshObjectName { get; }
    }
}

