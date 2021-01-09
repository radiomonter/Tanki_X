namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class MapNativeSoundsBehaviour : MonoBehaviour
    {
        [SerializeField]
        private FadeSoundFilter[] sounds;
        [CompilerGenerated]
        private static Action<FadeSoundFilter> <>f__am$cache0;
        [CompilerGenerated]
        private static Action<FadeSoundFilter> <>f__am$cache1;

        public void Play()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = s => s.Play(-1f);
            }
            this.sounds.ForEach<FadeSoundFilter>(<>f__am$cache0);
        }

        public void Stop()
        {
            base.enabled = true;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = s => s.Stop();
            }
            this.sounds.ForEach<FadeSoundFilter>(<>f__am$cache1);
        }

        private void Update()
        {
            if (base.transform.childCount <= 0)
            {
                DestroyObject(base.gameObject);
            }
        }
    }
}

