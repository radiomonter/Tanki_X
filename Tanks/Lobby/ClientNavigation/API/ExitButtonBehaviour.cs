namespace Tanks.Lobby.ClientNavigation.API
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class ExitButtonBehaviour : MonoBehaviour
    {
        [CompilerGenerated]
        private static UnityAction <>f__mg$cache0;

        private void Awake()
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new UnityAction(Application.Quit);
            }
            base.GetComponent<Button>().onClick.AddListener(<>f__mg$cache0);
        }
    }
}

