namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    public class BackhitDamageTutorialTrigger : MonoBehaviour
    {
        [SerializeField]
        private float showDelay = 60f;
        [HideInInspector]
        public bool canShow;
        private float timer;

        private void OnEnable()
        {
            this.timer = 0f;
        }

        private void Update()
        {
            this.timer += Time.deltaTime;
            if (this.timer >= this.showDelay)
            {
                this.canShow = true;
                base.GetComponent<TutorialShowTriggerComponent>().Triggered();
                base.enabled = false;
            }
        }
    }
}

