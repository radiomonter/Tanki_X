namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    public class TutorialWeaponHideTrigger : TutorialHideTriggerComponent
    {
        [SerializeField]
        private float showTime = 5f;
        private float timer;

        private void OnEnable()
        {
            this.timer = 0f;
        }

        private void Update()
        {
            this.timer += Time.deltaTime;
            if (this.timer >= this.showTime)
            {
                this.Triggered();
            }
        }
    }
}

