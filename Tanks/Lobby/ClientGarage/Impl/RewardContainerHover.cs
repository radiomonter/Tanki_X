namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class RewardContainerHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
    {
        public Animator animator;

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.animator.SetTrigger("Enter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.animator.SetTrigger("Exit");
        }
    }
}

