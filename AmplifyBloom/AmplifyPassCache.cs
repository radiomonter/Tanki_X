namespace AmplifyBloom
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AmplifyPassCache
    {
        [SerializeField]
        internal Vector4[] Offsets = new Vector4[0x10];
        [SerializeField]
        internal Vector4[] Weights = new Vector4[0x10];

        public void Destroy()
        {
            this.Offsets = null;
            this.Weights = null;
        }
    }
}

