namespace Tanks.Battle.ClientCore.API
{
    using System;
    using UnityEngine;

    public class GameObjectKeyCodeEnabler : MonoBehaviour
    {
        public KeyCode keyCode;
        public GameObject gameObject;

        private void Update()
        {
            if (Input.GetKeyDown(this.keyCode))
            {
                this.gameObject.SetActive(!this.gameObject.activeInHierarchy);
            }
        }
    }
}

