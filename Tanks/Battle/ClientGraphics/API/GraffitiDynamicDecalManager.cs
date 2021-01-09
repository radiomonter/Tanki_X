namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GraffitiDynamicDecalManager : DynamicDecalManager
    {
        public GraffitiDynamicDecalManager(GameObject root, int maxDecalCount, float decalLifeTimeKoeff, LinkedList<DecalEntry> decalsQueue) : base(root, maxDecalCount, decalLifeTimeKoeff, decalsQueue)
        {
        }

        public GameObject AddGraffiti(Mesh decalMesh, Material material, Color color, float lifeTime)
        {
            base.TrimQueue();
            base.SetMeshColorAndLifeTime(decalMesh, color, lifeTime * base.decalLifeTimeKoeff);
            float timeToDestroy = (Time.time + (lifeTime * base.decalLifeTimeKoeff)) + 2f;
            DecalEntry entry = base.CreateDecalEntry(decalMesh, material, timeToDestroy);
            entry.material.renderQueue = 0x898 + base.decalsQueue.Count;
            LinkedListNode<DecalEntry> node = base.decalsQueue.AddLast(entry);
            base.decalsCount++;
            return node.Value.gameObject;
        }

        public void RemoveDecal(GameObject decalObject)
        {
            LinkedListNode<DecalEntry> first = base.decalsQueue.First;
            int num = 0;
            while (first != null)
            {
                LinkedListNode<DecalEntry> next = first.Next;
                DecalEntry entry = first.Value;
                if (!entry.gameObject)
                {
                    base.decalsQueue.Remove(first);
                    base.decalsCount--;
                }
                else if (first.Value.gameObject == decalObject)
                {
                    Object.Destroy(first.Value.gameObject);
                    base.decalsQueue.Remove(first);
                    base.decalsCount--;
                }
                first.Value.material.renderQueue = 0x898 + num;
                num++;
                first = next;
            }
        }

        protected override DecalEntryType DecalType =>
            DecalEntryType.Graffiti;

        protected override string DecalMeshObjectName =>
            "Graffiti Mesh";
    }
}

