namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class TutorialKeymapComponent : UIBehaviour, Component
    {
        public GameObject prefab;
        public float showDelay = 30f;
        public float destroyDelay = 0.3333333f;
        private GameObject content;
        private float showTime = float.MaxValue;
        private float hideTime = float.MaxValue;
        private bool visible;

        public void ResetState()
        {
            this.showTime = float.MaxValue;
            this.hideTime = float.MaxValue;
            this.visible = false;
            if (this.content != null)
            {
                Destroy(this.content);
                this.content = null;
            }
        }

        private void Update()
        {
            if (this.Visible && (Time.time > (this.showTime + this.showDelay)))
            {
                this.Visible = false;
            }
            if (!this.visible && ((Time.time > (this.hideTime + this.destroyDelay)) && (this.content != null)))
            {
                Destroy(this.content);
                this.content = null;
            }
        }

        public bool Visible
        {
            get => 
                (this.content != null) && this.visible;
            set
            {
                if (value)
                {
                    if (this.content != null)
                    {
                        Destroy(this.content);
                    }
                    this.content = Instantiate<GameObject>(this.prefab, base.transform, false);
                }
                if (this.content != null)
                {
                    this.content.GetComponent<Animator>().SetTrigger(!value ? "HIDE" : "SHOW");
                }
                if (value)
                {
                    this.showTime = Time.time;
                }
                else
                {
                    this.hideTime = Time.time;
                }
                this.visible = value;
            }
        }
    }
}

