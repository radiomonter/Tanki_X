namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class AnimationGameObjectDisableBehaviour : MonoBehaviour
    {
        public GameObject gameObject;

        public void DisableGameObject()
        {
            this.gameObject.SetActive(false);
        }
    }
}

