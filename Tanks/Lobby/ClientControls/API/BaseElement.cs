namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class BaseElement
    {
        private HashSet<SpriteRequest> requests = new HashSet<SpriteRequest>();
        private HashSet<SpriteRequest> resolvedRequests = new HashSet<SpriteRequest>();
        private Dictionary<string, Sprite> resolvedSprites = new Dictionary<string, Sprite>();
        [SerializeField]
        private int canvasHeight;
        [SerializeField]
        private int size;
        [SerializeField]
        private List<AssetReference> skins = new List<AssetReference>();
        private int loadingSkinIndex;
        private bool loading;

        public void CancelAllRequests()
        {
            this.requests.Clear();
        }

        public void CancelRequest(SpriteRequest request)
        {
            this.requests.Remove(request);
        }

        private void ClearResolvedRequests()
        {
            foreach (SpriteRequest request in this.resolvedRequests)
            {
                this.requests.Remove(request);
            }
            this.resolvedRequests.Clear();
        }

        public void Init()
        {
            this.loadingSkinIndex = 0;
            this.loading = false;
            this.requests.Clear();
            this.resolvedSprites.Clear();
            this.resolvedRequests.Clear();
        }

        private void LoadNextSkin()
        {
            if (!this.loading)
            {
                this.loading = true;
                while ((this.loadingSkinIndex < this.skins.Count) && (this.skins[this.loadingSkinIndex].Reference != null))
                {
                    this.loadingSkinIndex++;
                }
                if (this.loadingSkinIndex < this.skins.Count)
                {
                    this.skins[this.loadingSkinIndex].OnLoaded = new Action<Object>(this.SkinLoaded);
                    this.skins[this.loadingSkinIndex].Load();
                }
            }
        }

        public void RequestSprite(SpriteRequest request)
        {
            Sprite sprite;
            this.resolvedSprites.TryGetValue(request.Uid, out sprite);
            if (sprite != null)
            {
                request.Resolve(sprite);
            }
            else
            {
                using (List<AssetReference>.Enumerator enumerator = this.skins.GetEnumerator())
                {
                    while (true)
                    {
                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                        AssetReference current = enumerator.Current;
                        if (current.Reference != null)
                        {
                            sprite = ((Skin) current.Reference).GetSprite(request.Uid);
                            if (sprite != null)
                            {
                                this.resolvedSprites.Add(request.Uid, sprite);
                                request.Resolve(sprite);
                                return;
                            }
                        }
                    }
                }
                if (!this.requests.Contains(request))
                {
                    this.requests.Add(request);
                }
                this.LoadNextSkin();
            }
        }

        private void SkinLoaded(Object result)
        {
            this.loading = false;
            this.TryResolveRequests((Skin) result);
            if (this.requests.Count > 0)
            {
                this.LoadNextSkin();
            }
        }

        private void TryResolveRequests(Skin skin)
        {
            Dictionary<SpriteRequest, Sprite> dictionary = new Dictionary<SpriteRequest, Sprite>();
            foreach (SpriteRequest request in this.requests)
            {
                if (request == null)
                {
                    this.resolvedRequests.Add(request);
                    continue;
                }
                Sprite sprite = skin.GetSprite(request.Uid);
                if (sprite != null)
                {
                    if (!this.resolvedSprites.ContainsKey(request.Uid))
                    {
                        this.resolvedSprites.Add(request.Uid, sprite);
                    }
                    dictionary.Add(request, sprite);
                }
            }
            foreach (KeyValuePair<SpriteRequest, Sprite> pair in dictionary)
            {
                SpriteRequest key = pair.Key;
                Sprite sprite = pair.Value;
                key.Resolve(sprite);
                this.resolvedRequests.Add(key);
            }
            this.ClearResolvedRequests();
        }

        public int CanvasHeight
        {
            get => 
                this.canvasHeight;
            set => 
                this.canvasHeight = value;
        }

        public int Size
        {
            get => 
                this.size;
            set => 
                this.size = value;
        }
    }
}

