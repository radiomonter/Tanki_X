namespace Edelweiss.TextureAtlas
{
    using Edelweiss.DecalSystem;
    using UnityEngine;

    public class TextureAtlasAsset : ScriptableObject
    {
        public Material material;
        public UVChannelModificationEnum uvChannelModification;
        public UVRectangle[] uvRectangles = new UVRectangle[0];
        public UVRectangle[] uv2Rectangles = new UVRectangle[0];
    }
}

