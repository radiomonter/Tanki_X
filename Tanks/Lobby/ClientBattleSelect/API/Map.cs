namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Map
    {
        private readonly Entity mapEntity;
        private Sprite loadPreview;

        public Map(Entity mapEntity)
        {
            this.mapEntity = mapEntity;
        }

        public Sprite LoadPreview
        {
            get
            {
                if (this.loadPreview == null)
                {
                    if (!this.mapEntity.HasComponent<MapLoadPreviewDataComponent>())
                    {
                        return null;
                    }
                    Texture2D data = (Texture2D) this.mapEntity.GetComponent<MapLoadPreviewDataComponent>().Data;
                    this.loadPreview = Sprite.Create(data, new Rect(0f, 0f, (float) data.width, (float) data.height), new Vector2(0.5f, 0.5f));
                }
                return this.loadPreview;
            }
        }

        public string Name =>
            this.mapEntity.HasComponent<DescriptionItemComponent>() ? this.mapEntity.GetComponent<DescriptionItemComponent>().Name : string.Empty;

        public List<string> FlavorTextList
        {
            get
            {
                if (this.mapEntity.HasComponent<FlavorListComponent>())
                {
                    return this.mapEntity.GetComponent<FlavorListComponent>().Collection;
                }
                return new List<string> { string.Empty };
            }
        }
    }
}

