namespace tanks.modules.tool.TankViewer.Scripts.API.ColoringEditor.ParamView
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Tanks.Tool.TankViewer.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class NormalMapView : MonoBehaviour
    {
        public Dropdown normalMapDropdown;
        public InputField normalScaleInput;
        public TextureDataSource dataSource;
        private bool inited;

        public void Awake()
        {
            this.dataSource.onStartUpdatingAction += delegate {
                this.inited = false;
                this.normalMapDropdown.ClearOptions();
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
            this.normalScaleInput.interactable = false;
            this.normalMapDropdown.interactable = false;
        }

        public void Enable()
        {
            this.normalMapDropdown.interactable = true;
            this.normalScaleInput.interactable = this.normalMapDropdown.value != 0;
        }

        public float GetNormalScale()
        {
            if (string.IsNullOrEmpty(this.normalScaleInput.text))
            {
                this.normalScaleInput.text = "1";
            }
            return float.Parse(this.normalScaleInput.text);
        }

        public TextureData GetSelectedNormalMap()
        {
            if (!this.inited)
            {
                return null;
            }
            if (this.normalMapDropdown.value <= 0)
            {
                return null;
            }
            int num = this.normalMapDropdown.value - 1;
            return this.dataSource.GetNormalMapsData()[num];
        }

        public void OnNormalDropdownChanged()
        {
            this.Enable();
        }

        public void SetNormalScale(float scale)
        {
            this.normalScaleInput.text = scale.ToString();
        }

        public void UpdateView()
        {
            this.normalMapDropdown.ClearOptions();
            List<TextureData> list = this.dataSource.GetData();
            this.normalMapDropdown.options.Add(new Dropdown.OptionData("none"));
            for (int i = 0; i < list.Count; i++)
            {
                TextureData data = list[i];
                Texture2D texture = data.texture2D;
                Sprite image = Sprite.Create(texture, new Rect(0f, 0f, (float) texture.width, (float) texture.height), new Vector2(0.5f, 0.5f));
                this.normalMapDropdown.options.Add(new Dropdown.OptionData(Path.GetFileName(data.filePath), image));
            }
            this.normalMapDropdown.value = 0;
            this.normalMapDropdown.RefreshShownValue();
            this.Enable();
            this.inited = true;
        }
    }
}

