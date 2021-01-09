namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class DistortionEffectBehaviour : MonoBehaviour
    {
        private const int DISTORTION_QUALITY_LEVEL = 2;
        [SerializeField]
        private GameObject[] distortionGameObjects;

        private void Awake()
        {
            bool flag = GraphicsSettings.INSTANCE.CurrentQualityLevel >= 2;
            int length = this.distortionGameObjects.Length;
            for (int i = 0; i < length; i++)
            {
                this.distortionGameObjects[i].SetActive(flag);
            }
        }
    }
}

