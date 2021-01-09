namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public static class SupplyEffectUtil
    {
        public static ParticleSystem[] InstantiateEffect(GameObject effectPrefab, Transform effectPoints)
        {
            ParticleSystem[] systemArray = new ParticleSystem[effectPoints.childCount];
            for (int i = 0; i < effectPoints.childCount; i++)
            {
                GameObject obj2 = Object.Instantiate<GameObject>(effectPrefab);
                Transform transform = obj2.transform;
                transform.SetParent(effectPoints.GetChild(i), false);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                ParticleSystem component = obj2.GetComponent<ParticleSystem>();
                component.Stop(true);
                systemArray[i] = component;
            }
            return systemArray;
        }
    }
}

