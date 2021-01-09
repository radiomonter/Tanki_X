namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    [RequireComponent(typeof(TemperatureVisualControllerComponent))]
    public class TemperatureEffects : MonoBehaviour, TemperatureChangeListener
    {
        [SerializeField]
        private GameObject freezingPrefab;
        [SerializeField]
        private GameObject burningPrefab;
        [SerializeField]
        private Transform mountPoint;
        private TemperatureEffect freezingEffect;
        private TemperatureEffect burningEffect;

        private void Awake()
        {
            this.burningEffect = this.InstansiateEffect(this.burningPrefab);
            this.freezingEffect = this.InstansiateEffect(this.freezingPrefab);
            base.GetComponent<TemperatureVisualControllerComponent>().listeners.Add(this);
        }

        private TemperatureEffect InstansiateEffect(GameObject prefab)
        {
            GameObject obj2 = Instantiate<GameObject>(prefab);
            obj2.transform.SetParent(this.mountPoint, false);
            obj2.SetActive(false);
            return obj2.GetComponent<TemperatureEffect>();
        }

        public void TemperatureChanged(float temperature)
        {
            this.UpdateBurningEffect(temperature);
            this.UpdateFreezingEffect(temperature);
        }

        private void UpdateBurningEffect(float temperature)
        {
            bool flag = temperature > 0f;
            this.burningEffect.gameObject.SetActive(flag);
            if (flag)
            {
                this.burningEffect.SetTemperature(temperature);
            }
        }

        private void UpdateFreezingEffect(float temperature)
        {
            bool flag = temperature < 0f;
            this.freezingEffect.gameObject.SetActive(flag);
            if (flag)
            {
                this.freezingEffect.SetTemperature(temperature);
            }
        }
    }
}

