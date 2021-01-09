namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Skin : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private string structureGuid;
        [SerializeField]
        private List<SkinSprite> sprites = new List<SkinSprite>();
        private Dictionary<string, SkinSprite> spritesMap = new Dictionary<string, SkinSprite>();

        public Sprite GetSprite(string uid) => 
            this.spritesMap.ContainsKey(uid) ? this.spritesMap[uid].Sprite : null;

        public void OnAfterDeserialize()
        {
            this.spritesMap = new Dictionary<string, SkinSprite>();
            foreach (SkinSprite sprite in this.sprites)
            {
                if (!string.IsNullOrEmpty(sprite.Uid))
                {
                    this.spritesMap.Add(sprite.Uid, sprite);
                }
            }
        }

        public void OnBeforeSerialize()
        {
            this.sprites = new List<SkinSprite>();
            foreach (SkinSprite sprite in this.spritesMap.Values)
            {
                if (sprite.Sprite != null)
                {
                    this.sprites.Add(sprite);
                }
            }
        }
    }
}

