using System;
using System.Collections;
using UnityEngine;

public class ChatSection : MonoBehaviour
{
    [SerializeField]
    private Transform header;
    [SerializeField]
    private Transform hideIcon;
    private bool hiden;

    public void SwitchHideState()
    {
        this.hiden = !this.hiden;
        this.hideIcon.transform.localScale = new Vector3(1f, !this.hiden ? ((float) (-1)) : ((float) 1), 1f) * 0.25f;
        IEnumerator enumerator = base.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                if (current != this.header)
                {
                    ((Transform) current).gameObject.SetActive(!this.hiden);
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
}

