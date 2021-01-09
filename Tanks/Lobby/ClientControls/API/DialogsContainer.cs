namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Linq;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class DialogsContainer : BehaviourComponent
    {
        public Transform[] ignoredChilds;

        public void CloseAll(string ignoredName = "")
        {
            IEnumerator enumerator = base.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    if (!string.Equals(ignoredName, current.gameObject.name) && ((this.ignoredChilds == null) || !this.ignoredChilds.Contains<Transform>(current)))
                    {
                        current.gameObject.SetActive(false);
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

        public T Get<T>() where T: MonoBehaviour => 
            base.GetComponentInChildren<T>(true);
    }
}

