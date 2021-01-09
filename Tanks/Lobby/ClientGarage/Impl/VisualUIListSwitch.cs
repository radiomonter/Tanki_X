namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    public class VisualUIListSwitch : MonoBehaviour
    {
        public void Animate()
        {
            if (base.gameObject.activeInHierarchy)
            {
                base.GetComponent<Animator>().SetTrigger("switch");
            }
        }

        public void OnEnable()
        {
            base.GetComponent<Animator>().SetTrigger("switch");
        }

        public void Switch()
        {
            base.GetComponentInParent<VisualUI>().Switch();
        }
    }
}

