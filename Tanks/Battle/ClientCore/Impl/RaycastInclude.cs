namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct RaycastInclude : IDisposable
    {
        private int[] initialLayers;
        private IEnumerable<GameObject> gameObjects;
        public RaycastInclude(IEnumerable<GameObject> gameObjects)
        {
            this.initialLayers = null;
            this.gameObjects = gameObjects;
            if (gameObjects != null)
            {
                this.IncludeGameObjectsToRaycast();
            }
        }

        public void Dispose()
        {
            if (this.gameObjects != null)
            {
                this.ReturnGameObjectsLayers();
            }
        }

        private void IncludeGameObjectsToRaycast()
        {
            int num = 0;
            this.initialLayers = new int[this.gameObjects.Count<GameObject>()];
            IEnumerator<GameObject> enumerator = this.gameObjects.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GameObject current = enumerator.Current;
                this.initialLayers[num++] = current.layer;
                current.layer = Layers.VISUAL_STATIC;
            }
        }

        private void ReturnGameObjectsLayers()
        {
            int num = 0;
            IEnumerator<GameObject> enumerator = this.gameObjects.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GameObject current = enumerator.Current;
                current.layer = this.initialLayers[num++];
            }
        }
    }
}

