namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class BulletHoleDecalManager : DynamicDecalManager
    {
        public BulletHoleDecalManager(GameObject root, int maxDecalCount, float decalLifeTimeKoeff, LinkedList<DecalEntry> decalsQueue) : base(root, maxDecalCount, decalLifeTimeKoeff, decalsQueue)
        {
        }

        public void AddDecal(Mesh decalMesh, Material material, Color color, float lifeTime)
        {
            base.TrimQueue();
            base.SetMeshColorAndLifeTime(decalMesh, color, lifeTime * base.decalLifeTimeKoeff);
            float timeToDestroy = (Time.time + (lifeTime * base.decalLifeTimeKoeff)) + 2f;
            base.decalsQueue.AddLast(base.CreateDecalEntry(decalMesh, material, timeToDestroy));
            base.decalsCount++;
            this.Optimize();
        }

        public void Optimize()
        {
            LinkedListNode<DecalEntry> first = base.decalsQueue.First;
            int num = 0;
            while (first != null)
            {
                LinkedListNode<DecalEntry> next = first.Next;
                DecalEntry entry = first.Value;
                if (Time.time > entry.timeToDestroy)
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
            DecalEntryType.BulletHole;

        protected override string DecalMeshObjectName =>
            "Decal Mesh";
    }
}

