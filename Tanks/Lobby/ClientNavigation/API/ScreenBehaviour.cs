namespace Tanks.Lobby.ClientNavigation.API
{
    using UnityEngine;

    public class ScreenBehaviour : StateMachineBehaviour
    {
        protected CanvasGroup GetCanvasGroup(GameObject gameObject)
        {
            CanvasGroup component = gameObject.GetComponent<CanvasGroup>();
            if (component == null)
            {
                component = gameObject.AddComponent<CanvasGroup>();
            }
            return component;
        }
    }
}

