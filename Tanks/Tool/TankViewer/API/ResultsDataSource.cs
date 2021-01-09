namespace Tanks.Tool.TankViewer.API
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;
    using UnityEngine.Networking;
    using YamlDotNet.Serialization;

    public class ResultsDataSource : MonoBehaviour
    {
        private List<string> folderNames = new List<string>();
        private List<ColoringComponent> coloringList = new List<ColoringComponent>();
        private Dictionary<UnityWebRequest, ColoringComponent> loadingTextures;
        private Dictionary<UnityWebRequest, ColoringComponent> loadingNormalMaps;
        private bool _isReady;
        public Action onChange;

        public void Add(string directoryName, ColoringComponent coloring)
        {
            this.folderNames.Add(directoryName);
            this.coloringList.Add(coloring);
            this.onChange();
        }

        private void Awake()
        {
            this.UpdateData();
        }

        public List<ColoringComponent> GetColoringComponents() => 
            this.IsReady ? this.coloringList : null;

        private Dictionary<object, object> GetConfig(string dir)
        {
            foreach (string str in Directory.GetFiles(dir))
            {
                if (Path.GetFileName(str).Equals("coloring.yml"))
                {
                    using (StreamReader reader = new StreamReader(str))
                    {
                        return (Dictionary<object, object>) new Deserializer(null, null, false, null).Deserialize(reader);
                    }
                }
            }
            return null;
        }

        public List<string> GetFolderNames() => 
            this.IsReady ? this.folderNames : null;

        public bool TexInfoValid(string dir, string texName)
        {
            if (string.IsNullOrEmpty(texName))
            {
                return true;
            }
            if (File.Exists(dir + "/" + texName))
            {
                return true;
            }
            Debug.Log("File not exist " + dir + "/" + texName);
            return false;
        }

        private bool TryComplete()
        {
            if ((this.loadingNormalMaps.Count != 0) || ((this.loadingTextures.Count != 0) || this.IsReady))
            {
                return false;
            }
            this.IsReady = true;
            if (this.onChange != null)
            {
                Debug.Log("call onChange");
                this.onChange();
            }
            return true;
        }

        public void Update()
        {
            if ((!this.IsReady && (this.loadingNormalMaps != null)) && !this.TryComplete())
            {
                List<UnityWebRequest> list = this.loadingTextures.Keys.ToList<UnityWebRequest>();
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    UnityWebRequest key = list[i];
                    if (key.isError)
                    {
                        Debug.Log(key.error);
                        this.loadingTextures.Remove(key);
                    }
                    else if (key.isDone)
                    {
                        this.loadingTextures[key].coloringTexture = TextureLoadingUtility.CreateTextureWithGamma(((DownloadHandlerTexture) key.downloadHandler).texture);
                        this.loadingTextures.Remove(key);
                    }
                }
                list = this.loadingNormalMaps.Keys.ToList<UnityWebRequest>();
                for (int j = list.Count - 1; j >= 0; j--)
                {
                    UnityWebRequest key = list[j];
                    if (key.isError)
                    {
                        Debug.Log(key.error);
                        this.loadingNormalMaps.Remove(key);
                    }
                    else if (key.isDone)
                    {
                        this.loadingNormalMaps[key].coloringNormalMap = TextureLoadingUtility.CreateNormalMap(((DownloadHandlerTexture) key.downloadHandler).texture);
                        this.loadingNormalMaps.Remove(key);
                    }
                }
            }
        }

        public void UpdateData()
        {
            Dictionary<object, object> config;
            ColoringComponent component;
            this.IsReady = false;
            string[] strArray = Directory.GetDirectories(".", "Results_*", SearchOption.TopDirectoryOnly);
            this.folderNames.Clear();
            this.coloringList.Clear();
            this.loadingTextures = new Dictionary<UnityWebRequest, ColoringComponent>();
            this.loadingNormalMaps = new Dictionary<UnityWebRequest, ColoringComponent>();
            string[] strArray2 = strArray;
            int index = 0;
            goto TR_0014;
        TR_0001:
            index++;
        TR_0014:
            while (true)
            {
                if (index >= strArray2.Length)
                {
                    this.TryComplete();
                    return;
                }
                string dir = strArray2[index];
                config = this.GetConfig(dir);
                if (config == null)
                {
                    goto TR_0001;
                }
                else
                {
                    config = (Dictionary<object, object>) config.First<KeyValuePair<object, object>>().Value;
                    string texName = config["coloringTexture"] as string;
                    string str3 = config["coloringNormalMap"] as string;
                    if (!this.TexInfoValid(dir, texName) || !this.TexInfoValid(dir, str3))
                    {
                        goto TR_0001;
                    }
                    else
                    {
                        Color color;
                        component = new GameObject("ResultColoring").AddComponent<ColoringComponent>();
                        this.folderNames.Add(dir);
                        this.coloringList.Add(component);
                        ColorUtility.TryParseHtmlString("#" + ((string) config["color"]), out color);
                        component.color = color;
                        string str4 = Path.GetFullPath(dir) + "/";
                        if (!string.IsNullOrEmpty(texName))
                        {
                            UnityWebRequest texture = UnityWebRequest.GetTexture(str4 + texName);
                            texture.Send();
                            this.loadingTextures.Add(texture, component);
                        }
                        if (!string.IsNullOrEmpty(str3))
                        {
                            UnityWebRequest texture = UnityWebRequest.GetTexture(str4 + str3);
                            texture.Send();
                            this.loadingNormalMaps.Add(texture, component);
                        }
                        string str5 = (string) config["coloringTextureAlphaMode"];
                        if (str5 != null)
                        {
                            if (str5 == "AS_EMISSION_MASK")
                            {
                                component.coloringTextureAlphaMode = ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_EMISSION_MASK;
                                break;
                            }
                            if (str5 == "AS_SMOOTHNESS")
                            {
                                component.coloringTextureAlphaMode = ColoringComponent.COLORING_MAP_ALPHA_MODE.AS_SMOOTHNESS;
                                break;
                            }
                        }
                        component.coloringTextureAlphaMode = ColoringComponent.COLORING_MAP_ALPHA_MODE.NONE;
                    }
                }
                break;
            }
            component.coloringNormalScale = float.Parse((string) config["coloringNormalScale"]);
            component.metallic = float.Parse((string) config["metallic"]);
            component.overwriteSmoothness = bool.Parse((string) config["overwriteSmoothness"]);
            component.smoothnessStrength = float.Parse((string) config["smoothnessStrength"]);
            component.useColoringIntensityThreshold = bool.Parse((string) config["useColoringIntensityThreshold"]);
            component.coloringMaskThreshold = float.Parse((string) config["coloringMaskThreshold"]);
            goto TR_0001;
        }

        public bool IsReady
        {
            get => 
                this._isReady;
            private set => 
                this._isReady = value;
        }
    }
}

