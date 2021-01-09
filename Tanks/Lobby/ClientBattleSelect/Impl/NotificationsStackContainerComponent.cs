namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class NotificationsStackContainerComponent : UIBehaviour, Component
    {
        [SerializeField]
        private float resetDelay = 15f;
        [SerializeField]
        private float yOffset = 100f;
        private float lastResetTime;
        private float lastOffset;

        public unsafe GameObject CreateNotification(GameObject prefab)
        {
            if ((this.lastResetTime + this.resetDelay) >= Time.time)
            {
                this.lastOffset += this.yOffset;
            }
            else
            {
                this.lastOffset = 0f;
                this.lastResetTime = Time.time;
            }
            GameObject obj2 = Instantiate<GameObject>(prefab, base.transform, false);
            RectTransform transform = (RectTransform) obj2.transform;
            Vector2 anchoredPosition = transform.anchoredPosition;
            Vector2* vectorPtr1 = &anchoredPosition;
            vectorPtr1->y += this.lastOffset;
            transform.anchoredPosition = anchoredPosition;
            obj2.SetActive(true);
            return obj2;
        }
    }
}

