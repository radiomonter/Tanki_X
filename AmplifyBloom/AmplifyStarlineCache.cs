namespace AmplifyBloom
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AmplifyStarlineCache
    {
        [SerializeField]
        internal AmplifyPassCache[] Passes = new AmplifyPassCache[4];

        public AmplifyStarlineCache()
        {
            for (int i = 0; i < 4; i++)
            {
                this.Passes[i] = new AmplifyPassCache();
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < 4; i++)
            {
                this.Passes[i].Destroy();
            }
            this.Passes = null;
        }
    }
}

