namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class ChatContentGUIComponent : MonoBehaviour, Component
    {
        [FormerlySerializedAs("messageAsset"), SerializeField]
        private GameObject messagePrefab;

        public void ClearMessages()
        {
            IEnumerator enumerator = base.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    if (current.GetComponent<ChatMessageUIComponent>() != null)
                    {
                        Destroy(current.gameObject);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public GameObject MessagePrefab =>
            this.messagePrefab;
    }
}

