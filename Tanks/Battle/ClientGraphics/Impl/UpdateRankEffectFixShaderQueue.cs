namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectFixShaderQueue : MonoBehaviour
    {
        public int AddQueue = 1;

        private void SetProjectorQueue()
        {
            Material material = base.GetComponent<Projector>().material;
            material.renderQueue += this.AddQueue;
        }

        private void Start()
        {
            if (base.GetComponent<Renderer>() == null)
            {
                base.Invoke("SetProjectorQueue", 0.1f);
            }
            else
            {
                Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
                sharedMaterial.renderQueue += this.AddQueue;
            }
        }

        private void Update()
        {
        }
    }
}

