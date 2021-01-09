namespace Tanks.Tool.TankViewer.API
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Networking;

    public class TextureDataSource : MonoBehaviour
    {
        private List<string> filePaths;
        private string basePath;
        private UnityWebRequest webRequest;
        private int index;
        private List<TextureData> data = new List<TextureData>();
        public Action onStartUpdatingAction;
        public Action onCompleteUpdatingAction;
        private List<TextureData> convertedToNormalMap = new List<TextureData>();

        private void Clean()
        {
            for (int i = 0; i < this.data.Count; i++)
            {
                Destroy(this.data[i].texture2D);
                if (this.convertedToNormalMap[i] != null)
                {
                    Destroy(this.convertedToNormalMap[i].texture2D);
                }
            }
            this.data.Clear();
            this.convertedToNormalMap.Clear();
        }

        private void Complete()
        {
            this.webRequest = null;
            this.TexturesAreReady = true;
            this.onCompleteUpdatingAction();
        }

        public List<TextureData> GetData() => 
            this.data;

        public List<TextureData> GetNormalMapsData() => 
            this.convertedToNormalMap;

        private void LoadNextTexture()
        {
            this.webRequest = UnityWebRequest.GetTexture(this.filePaths[this.index]);
            this.webRequest.Send();
        }

        private void Update()
        {
            if ((this.webRequest != null) && this.webRequest.isError)
            {
                Debug.Log(this.webRequest.error + " url:  " + this.webRequest.url);
            }
            if ((this.webRequest != null) && (this.webRequest.isDone && !this.TexturesAreReady))
            {
                Texture2D texture = ((DownloadHandlerTexture) this.webRequest.downloadHandler).texture;
                this.data.Add(new TextureData(this.filePaths[this.index], TextureLoadingUtility.CreateTextureWithGamma(texture)));
                this.convertedToNormalMap.Add(new TextureData(this.filePaths[this.index], TextureLoadingUtility.CreateNormalMap(texture)));
                Destroy(texture);
                this.index++;
                if (this.index < this.filePaths.Count)
                {
                    this.LoadNextTexture();
                }
                else
                {
                    this.Complete();
                }
            }
        }

        public void UpdateData()
        {
            this.TexturesAreReady = false;
            this.Clean();
            this.onStartUpdatingAction();
            this.basePath = Path.GetFullPath("workspace");
            if (!Directory.Exists(this.basePath))
            {
                Directory.CreateDirectory(this.basePath);
            }
            this.filePaths = Directory.GetFiles(this.basePath).ToList<string>();
            for (int i = this.filePaths.Count - 1; i >= 0; i--)
            {
                string str = Path.GetExtension(this.filePaths[i]).ToLower();
                if (!str.Equals(".png") && !str.Equals(".jpg"))
                {
                    this.filePaths.RemoveAt(i);
                }
            }
            this.index = 0;
            if (this.filePaths.Count > 0)
            {
                this.LoadNextTexture();
            }
            else
            {
                this.Complete();
            }
        }

        public bool TexturesAreReady { get; private set; }
    }
}

