namespace Tanks.Lobby.ClientEntrance.Impl
{
    using System;
    using UnityEngine;

    public class AnimatedLogo : MonoBehaviour
    {
        public Material videoTextureMaterial;

        private void Start()
        {
            MovieTexture mainTexture = (MovieTexture) this.videoTextureMaterial.mainTexture;
            mainTexture.loop = true;
            mainTexture.Play();
        }
    }
}

