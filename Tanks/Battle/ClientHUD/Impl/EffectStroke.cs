namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using UnityEngine;

    public class EffectStroke : MonoBehaviour
    {
        private void OnEnable()
        {
            base.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, (float) (Random.Range(0, 4) * 90));
        }
    }
}

