namespace Tanks.Tool.TankViewer.API
{
    using System;
    using System.IO;
    using UnityEngine;

    public class TextureData
    {
        public string filePath;
        public Texture2D texture2D;
        public string name;

        public TextureData(string filePath, Texture2D texture2D)
        {
            this.filePath = filePath;
            this.texture2D = texture2D;
            this.name = Path.GetFileName(filePath);
        }
    }
}

