namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class UpdateRankEffectInvokeInterval : MonoBehaviour
    {
        public GameObject GO;
        public float Interval = 0.3f;
        public float Duration = 3f;
        private List<GameObject> goInstances;
        private UpdateRankEffectSettings effectSettings;
        private int goIndexActivate;
        private int goIndexDeactivate;
        private bool isInitialized;
        private int count;

        private void effectSettings_EffectDeactivated(object sender, EventArgs e)
        {
            (sender as UpdateRankEffectSettings).transform.position = base.transform.position;
            if (this.goIndexDeactivate < (this.count - 1))
            {
                this.goIndexDeactivate++;
            }
            else
            {
                this.effectSettings.Deactivate();
                this.goIndexDeactivate = 0;
            }
        }

        private void GetEffectSettingsComponent(Transform tr)
        {
            Transform parent = tr.parent;
            if (parent != null)
            {
                this.effectSettings = parent.GetComponentInChildren<UpdateRankEffectSettings>();
                if (this.effectSettings == null)
                {
                    this.GetEffectSettingsComponent(parent.transform);
                }
            }
        }

        private void InvokeAll()
        {
            for (int i = 0; i < this.count; i++)
            {
                base.Invoke("InvokeInstance", i * this.Interval);
            }
        }

        private void InvokeInstance()
        {
            this.goInstances[this.goIndexActivate].SetActive(true);
            this.goIndexActivate = (this.goIndexActivate < (this.goInstances.Count - 1)) ? (this.goIndexActivate + 1) : 0;
        }

        private void OnDisable()
        {
        }

        private void OnEnable()
        {
            if (this.isInitialized)
            {
                this.InvokeAll();
            }
        }

        private void Start()
        {
            this.GetEffectSettingsComponent(base.transform);
            this.goInstances = new List<GameObject>();
            this.count = (int) (this.Duration / this.Interval);
            for (int i = 0; i < this.count; i++)
            {
                Quaternion rotation = new Quaternion();
                GameObject item = Instantiate<GameObject>(this.GO, base.transform.position, rotation);
                item.transform.parent = base.transform;
                UpdateRankEffectSettings component = item.GetComponent<UpdateRankEffectSettings>();
                component.Target = this.effectSettings.Target;
                component.IsHomingMove = this.effectSettings.IsHomingMove;
                component.MoveDistance = this.effectSettings.MoveDistance;
                component.MoveSpeed = this.effectSettings.MoveSpeed;
                component.CollisionEnter += (n, e) => this.effectSettings.OnCollisionHandler(e);
                component.ColliderRadius = this.effectSettings.ColliderRadius;
                component.EffectRadius = this.effectSettings.EffectRadius;
                component.EffectDeactivated += new EventHandler(this.effectSettings_EffectDeactivated);
                this.goInstances.Add(item);
                item.SetActive(false);
            }
            this.InvokeAll();
            this.isInitialized = true;
        }
    }
}

