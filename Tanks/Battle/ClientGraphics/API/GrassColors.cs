namespace Tanks.Battle.ClientGraphics.API
{
    using System.Collections.Generic;
    using UnityEngine;

    public class GrassColors : MonoBehaviour
    {
        public List<Color> colors = new List<Color>();

        public Color GetRandomColor() => 
            this.colors[Random.Range(0, this.colors.Count)];
    }
}

