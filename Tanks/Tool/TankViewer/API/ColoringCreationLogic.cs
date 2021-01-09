namespace Tanks.Tool.TankViewer.API
{
    using System;
    using System.IO;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;
    using YamlDotNet.Serialization;

    public class ColoringCreationLogic : MonoBehaviour
    {
        public TankConstructor tankConstructor;
        public ResultsDataSource resultsDataSource;
        private ColoringComponent newColoring;
        private TextureData coloringTexture;
        private TextureData normalMap;

        public void Cancel()
        {
            if (this.newColoring != null)
            {
                Destroy(this.newColoring.gameObject);
                this.newColoring = null;
            }
        }

        public void CleanTextures()
        {
            if (this.newColoring != null)
            {
                this.newColoring.coloringTexture = null;
                this.newColoring.coloringNormalMap = null;
                this.tankConstructor.ChangeColoring(this.newColoring);
            }
        }

        public ColoringComponent CreateNewColoring()
        {
            this.Cancel();
            this.newColoring = new GameObject("NewColoring").AddComponent<ColoringComponent>();
            this.tankConstructor.ChangeColoring(this.newColoring);
            return this.newColoring;
        }

        public void Save()
        {
            DirectoryInfo info = Directory.CreateDirectory("Results_" + DateTime.Now.ToString("HH-mm_dd-MM-yyyy"));
            if (this.coloringTexture != null)
            {
                File.Copy(this.coloringTexture.filePath, info.FullName + "/" + this.coloringTexture.name);
            }
            if ((this.normalMap != null) && !this.normalMap.filePath.Equals(this.coloringTexture.filePath))
            {
                File.Copy(this.normalMap.filePath, info.FullName + "/" + this.normalMap.name);
            }
            StreamWriter writer = File.CreateText(info.FullName + "/coloring.yml");
            new Serializer(SerializationOptions.EmitDefaults | SerializationOptions.DisableAliases, null, null).Serialize(writer, new <>__AnonType1<<>__AnonType0<string, string, string, string, float, float, bool, float, bool, float>>(new <>__AnonType0<string, string, string, string, float, float, bool, float, bool, float>(ColorUtility.ToHtmlStringRGB((Color) this.newColoring.color), (this.coloringTexture == null) ? string.Empty : this.coloringTexture.name, this.newColoring.coloringTextureAlphaMode.ToString(), (this.normalMap == null) ? string.Empty : this.normalMap.name, this.newColoring.coloringNormalScale, this.newColoring.metallic, this.newColoring.overwriteSmoothness, this.newColoring.smoothnessStrength, this.newColoring.useColoringIntensityThreshold, this.newColoring.coloringMaskThreshold)));
            writer.Close();
            this.resultsDataSource.Add(info.Name, this.newColoring);
        }

        public void UpdateColoring(ColoringComponent coloringComponent)
        {
            this.tankConstructor.ChangeColoring(coloringComponent);
        }

        public void UpdateColoring(Color color, TextureData coloringTexture, ColoringComponent.COLORING_MAP_ALPHA_MODE alphaMode, TextureData normalMap, float normalScale, float metallic, bool overrideSmoothness, float smoothnessStrenght, bool useIntensityThreshold, float intensityThreshold)
        {
            this.coloringTexture = coloringTexture;
            this.normalMap = normalMap;
            this.newColoring.color = color;
            this.newColoring.coloringTextureAlphaMode = alphaMode;
            this.newColoring.coloringTexture = (coloringTexture == null) ? null : coloringTexture.texture2D;
            this.newColoring.coloringNormalMap = (normalMap == null) ? null : normalMap.texture2D;
            this.newColoring.coloringNormalScale = normalScale;
            this.newColoring.metallic = metallic;
            this.newColoring.overwriteSmoothness = overrideSmoothness;
            this.newColoring.smoothnessStrength = smoothnessStrenght;
            this.newColoring.useColoringIntensityThreshold = useIntensityThreshold;
            this.newColoring.coloringMaskThreshold = intensityThreshold;
            this.tankConstructor.ChangeColoring(this.newColoring);
        }
    }
}

