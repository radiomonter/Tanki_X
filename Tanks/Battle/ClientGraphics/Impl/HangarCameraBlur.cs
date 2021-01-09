namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class HangarCameraBlur : MonoBehaviour
    {
        private void OnEnable()
        {
            HangarCameraPostProcessingActivator.ActivePanel = base.gameObject;
        }
    }
}

