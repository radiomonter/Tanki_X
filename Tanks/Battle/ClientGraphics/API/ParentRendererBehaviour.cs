namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using UnityEngine;

    public class ParentRendererBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Renderer parentRenderer;

        public Renderer ParentRenderer
        {
            get => 
                this.parentRenderer;
            set => 
                this.parentRenderer = value;
        }
    }
}

