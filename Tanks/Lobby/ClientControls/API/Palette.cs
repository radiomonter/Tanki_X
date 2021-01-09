namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class Palette : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<ColorNode> nodes = new List<ColorNode>();
        private Dictionary<int, ColorNode> colorsMap = new Dictionary<int, ColorNode>();

        public Color Apply(int uid, Color color)
        {
            if (this.colorsMap.ContainsKey(uid))
            {
                ColorNode node = this.colorsMap[uid];
                if (node.useColor)
                {
                    color.r = node.color.r;
                    color.g = node.color.g;
                    color.b = node.color.b;
                }
                if (node.useAlpha)
                {
                    color.a = node.color.a;
                }
            }
            return color;
        }

        public Color Get(int uid)
        {
            Color color = new Color(float.NaN, float.NaN, float.NaN, float.NaN);
            return this.Apply(uid, color);
        }

        public void OnAfterDeserialize()
        {
            this.colorsMap.Clear();
            for (int i = 0; i < this.nodes.Count; i++)
            {
                this.colorsMap.Add(this.nodes[i].uid, this.nodes[i]);
            }
        }

        public void OnBeforeSerialize()
        {
        }

        [Serializable]
        public class ColorNode
        {
            [SerializeField]
            public Color color;
            [SerializeField]
            public string name;
            [SerializeField]
            public int uid;
            [SerializeField]
            public bool useAlpha;
            [SerializeField]
            public bool useColor;

            public override string ToString() => 
                $"Color: {this.color}, Name: {this.name}, Uid: {this.uid}, UseAlpha: {this.useAlpha}, UseColor: {this.useColor}";
        }
    }
}

