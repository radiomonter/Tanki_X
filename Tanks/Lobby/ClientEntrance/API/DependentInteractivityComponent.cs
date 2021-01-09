namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Selectable))]
    public class DependentInteractivityComponent : MonoBehaviour, Component
    {
        public List<InteractivityPrerequisiteComponent> prerequisitesObjects;

        public virtual void HideCarouselInteractable(bool value)
        {
            base.transform.parent.localScale = value ? new Vector3(1f, 1f, 1f) : new Vector3(0f, 0f, 0f);
        }

        public virtual void HideCheckbox(bool value)
        {
            base.transform.localScale = value ? new Vector3(1f, 1f, 1f) : new Vector3(0f, 0f, 0f);
        }

        public virtual void SetCarouselInteractable(bool value)
        {
            base.GetComponent<CarouselComponent>().BackButton.GetComponent<Button>().interactable = value;
            base.GetComponent<CarouselComponent>().FrontButton.GetComponent<Button>().interactable = value;
        }

        public virtual void SetInteractable(bool value)
        {
            base.GetComponent<Selectable>().interactable = value;
        }
    }
}

