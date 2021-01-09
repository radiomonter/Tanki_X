namespace Tanks.Tool.TankViewer.API
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;
    using UnityEngine.UI;

    public class TextureView : MonoBehaviour
    {
        public Dropdown textureDropdown;
        public Dropdown alphaModeDropdown;
        public TextureDataSource dataSource;
        private bool inited;

        public void Awake()
        {
            this.dataSource.onStartUpdatingAction += delegate {
                this.inited = false;
                this.textureDropdown.ClearOptions();
                this.Disable();
            };
            this.dataSource.onCompleteUpdatingAction += new Action(this.UpdateView);
            if (this.dataSource.TexturesAreReady)
            {
                this.UpdateView();
            }
        }

        public void Disable()
        {
            this.alphaModeDropdown.interactable = false;
            this.textureDropdown.interactable = false;
        }

        public void Enable()
        {
            this.textureDropdown.interactable = true;
            this.alphaModeDropdown.interactable = this.textureDropdown.value != 0;
        }

        public ColoringComponent.COLORING_MAP_ALPHA_MODE GetAlphaMode()
        {
            int num = this.alphaModeDropdown.value;
            return ((num == 0) ? ColoringComponent.COLORING_MAP_ALPHA_MODE.NONE : ((num == 1) ? ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_EMISSION_MASK : ((num == 2) ? ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_SMOOTHNESS : ColoringComponent.COLORING_MAP_ALPHA_MODE.NONE)));
        }

        public TextureData GetSelectedTexture() => 
            this.inited ? ((this.textureDropdown.value <= 0) ? null : this.dataSource.GetData()[this.textureDropdown.value - 1]) : null;

        public void OnTextureDropdownChanged()
        {
            this.Enable();
        }

        public void SetAlphaMode(ColoringComponent.COLORING_MAP_ALPHA_MODE alphaMode)
        {
            if (alphaMode == ColoringComponent.COLORING_MAP_ALPHA_MODE.NONE)
            {
                this.alphaModeDropdown.value = 0;
            }
            else if (alphaMode == ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_EMISSION_MASK)
            {
                this.alphaModeDropdown.value = 1;
            }
            else if (alphaMode == ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_SMOOTHNESS)
            {
                this.alphaModeDropdown.value = 2;
            }
        }

        public void UpdateView()
        {
            this.textureDropdown.ClearOptions();
            List<TextureData> list = this.dataSource.GetData();
            this.textureDropdown.options.Add(new Dropdown.OptionData("none"));
            for (int i = 0; i < list.Count; i++)
            {
                TextureData data = list[i];
                Texture2D texture = data.texture2D;
                Sprite image = Sprite.Create(texture, new Rect(0f, 0f, (float) texture.width, (float) texture.height), new Vector2(0.5f, 0.5f));
                this.textureDropdown.options.Add(new Dropdown.OptionData(Path.GetFileName(data.filePath), image));
            }
            this.textureDropdown.value = 0;
            this.textureDropdown.RefreshShownValue();
            this.Enable();
            this.inited = true;
        }
    }
}

