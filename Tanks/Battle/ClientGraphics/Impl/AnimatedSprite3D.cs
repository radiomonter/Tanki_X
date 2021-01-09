namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AnimatedSprite3D : Sprite3D
    {
        public int tilesX = 1;
        public int tilesY = 1;
        public int fps = 0x18;
        public int framesCount;
        public bool loop = true;
        private Vector2 tileSize;
        private int lastIndex = -1;
        private float initTime;

        private void AnimateMaterialTile()
        {
            this.tileSize = new Vector2(1f / ((float) this.tilesX), 1f / ((float) this.tilesY));
            int num = (this.framesCount <= 0) ? (this.tilesX * this.tilesY) : this.framesCount;
            int num2 = (int) ((UnityTime.time - this.initTime) * this.fps);
            if (this.loop)
            {
                num2 = num2 % num;
            }
            else if (num2 >= num)
            {
                num2 = num - 1;
            }
            if (num2 != this.lastIndex)
            {
                int num4 = num2 / this.tilesY;
                Vector2 vector = new Vector2((num2 % this.tilesX) * this.tileSize.x, (1f - this.tileSize.y) - (num4 * this.tileSize.y));
                base.material.SetTextureOffset("_MainTex", vector);
                base.material.SetTextureScale("_MainTex", this.tileSize);
                this.lastIndex = num2;
            }
        }

        public override void Draw()
        {
            base.useInstanceMaterial = true;
            this.AnimateMaterialTile();
            base.Draw();
        }

        public void ResetMaterial(Material newMaterial)
        {
            base.assetMaterial = newMaterial;
            base.material = new Material(newMaterial);
            this.lastIndex = -1;
        }

        protected void Start()
        {
            base.Start();
            base.useInstanceMaterial = true;
            base.UpdateMaterial();
            this.initTime = UnityTime.time;
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }
    }
}

