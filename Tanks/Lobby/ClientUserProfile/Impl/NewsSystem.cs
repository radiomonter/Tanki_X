namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientResources.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using SharpCompress.Compressor;
    using SharpCompress.Compressor.Deflate;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class NewsSystem : ECSSystem
    {
        private static string ID = "{ID}";
        private static string RANK = "{RANK}";
        private static string UID = "{UID}";
        private static string LOCALE = "{LOCALE}";
        private HashSet<long> seenNews = new HashSet<long>();
        private bool newsContainerSeen;
        private GameObject badge;
        private Dictionary<string, Texture> textureCache = new Dictionary<string, Texture>();

        [OnEventFire]
        public void AssetLoadComplete(NodeAddedEvent e, NewsItemWithPreviewDataNode newsItem)
        {
            base.Log.InfoFormat("AssetLoadComplete {0}", newsItem.newsItemImageData.Data);
            Texture2D data = (Texture2D) newsItem.newsItemImageData.Data;
            newsItem.newsItemUI.ImageContainer.SetRawImage(data);
            this.ConfigureImage(newsItem);
        }

        [OnEventFire]
        public void CetralIconLoadComplete(NodeAddedEvent e, NewsItemWithCetralIconNode newsItem)
        {
            base.Log.InfoFormat("CetralIconLoadComplete {0}", newsItem.newsItemCentralIconData.Data);
            Texture2D data = (Texture2D) newsItem.newsItemCentralIconData.Data;
            newsItem.newsItemUI.SetCentralIcon(data);
        }

        private void ConfigureImage(NewsItemWithUINode newsItem)
        {
            newsItem.newsItemUI.ImageContainer.FitInParent = newsItem.newsItem.Data.PreviewImageFitInParent;
        }

        [OnEventFire]
        public void CreateUI(NodeAddedEvent e, [Combine] NewsItemNode newsItem, NewsContainerNode container)
        {
            this.CreateUIIfNeed(newsItem, container);
        }

        private void CreateUIIfNeed(NewsItemNode newsItem, NewsContainerNode container)
        {
            if (this.NeedHideNewsItem(newsItem))
            {
                base.Log.InfoFormat("Hide newsItem: {0}", newsItem);
            }
            else
            {
                Transform containerTransform = container.newsContainer.GetContainerTransform(newsItem.newsItem.Data.Layout);
                if (containerTransform == null)
                {
                    base.Log.ErrorFormat("Container for NewsItem not found: {0}", newsItem.newsItem.Data);
                }
                else
                {
                    GameObject itemObject = Object.Instantiate<GameObject>(container.newsContainer.newsItemPrefab);
                    itemObject.GetComponent<RectTransform>().SetParent(containerTransform, false);
                    itemObject.GetComponent<EntityBehaviour>().BuildEntity(newsItem.Entity);
                    this.seenNews.Add(newsItem.Entity.Id);
                    this.SetAsFirstSiblingIfLessShown(newsItem, containerTransform, itemObject);
                }
            }
        }

        [OnEventFire, Conditional("DEBUG")]
        public void Debug(NodeAddedEvent e, NewsItemNode newsItem)
        {
            base.Log.InfoFormat("Add NewsItem: {0}", newsItem.newsItem.Data);
        }

        private static byte[] DecompressGzip(byte[] bytes, int bufferSize)
        {
            byte[] buffer;
            using (GZipStream stream = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
            {
                byte[] buffer2 = new byte[bufferSize];
                int offset = 0;
                int num2 = 0;
                while (true)
                {
                    num2 = stream.Read(buffer2, offset, buffer2.Length - offset);
                    if (num2 <= 0)
                    {
                        buffer = new byte[offset];
                        Array.Copy(buffer2, buffer, offset);
                        break;
                    }
                    offset += num2;
                }
            }
            return buffer;
        }

        [OnEventFire]
        public void DeleteUI1(NodeRemoveEvent e, NewsItemNode newsItem, [JoinAll] NewsContainerNode container)
        {
            this.DeleteUIIfExists(newsItem, true);
        }

        [OnEventFire]
        public void DeleteUI2(NodeRemoveEvent e, NewsContainerNode container, [JoinAll, Combine] NewsItemNode newsItem)
        {
            this.DeleteUIIfExists(newsItem, false);
        }

        private void DeleteUIIfExists(NewsItemNode newsItem, bool immediate)
        {
            bool flag = newsItem.Entity.HasComponent<NewsItemUIComponent>();
            base.Log.InfoFormat("DeleteUIIfExists: {0} {1}", flag, newsItem);
            if (flag)
            {
                NewsItemUIComponent component = newsItem.Entity.GetComponent<NewsItemUIComponent>();
                component.gameObject.GetComponent<EntityBehaviour>().RemoveUnityComponentsFromEntity();
                if (immediate)
                {
                    Object.DestroyImmediate(component.gameObject);
                }
                else
                {
                    Object.Destroy(component.gameObject);
                }
            }
        }

        [OnEventFire]
        public void InitUI(NodeAddedEvent e, NewsItemWithUINode newsItem)
        {
            NewsItem data = newsItem.newsItem.Data;
            newsItem.newsItemUI.HeaderText = data.HeaderText;
            if (!string.IsNullOrEmpty(data.Tooltip))
            {
                newsItem.newsItemUI.Tooltip = data.Tooltip;
            }
            if (data.Date.Year > 0x7d0)
            {
                newsItem.newsItemUI.DateText = data.Date.ToString("dd.MM.yyyy");
            }
            if (!string.IsNullOrEmpty(data.PreviewImageGuid))
            {
                base.Log.InfoFormat("Request load PreviewImage: {0}", data.PreviewImageGuid);
                base.ScheduleEvent(new AssetRequestEvent().Init<NewsItemImageDataComponent>(data.PreviewImageGuid), newsItem);
            }
            else if (!string.IsNullOrEmpty(data.PreviewImageUrl))
            {
                Texture texture;
                if (this.textureCache.TryGetValue(data.PreviewImageUrl, out texture))
                {
                    base.Log.InfoFormat("Get PreviewImage from cache: {0}", data.PreviewImageUrl);
                    this.SetImage(newsItem, texture);
                }
                else
                {
                    base.Log.InfoFormat("Load PreviewImage: {0}", data.PreviewImageUrl);
                    if (!newsItem.Entity.HasComponent<UrlComponent>())
                    {
                        UrlComponent component = new UrlComponent {
                            Url = data.PreviewImageUrl,
                            Caching = false,
                            NoErrorEvent = true
                        };
                        newsItem.Entity.AddComponent(component);
                    }
                }
            }
            if (!string.IsNullOrEmpty(data.CentralIconGuid))
            {
                base.Log.InfoFormat("Request load CentralIcon: {0}", data.CentralIconGuid);
                base.ScheduleEvent(new AssetRequestEvent().Init<NewsItemCentralIconDataComponent>(data.CentralIconGuid), newsItem);
            }
        }

        private static bool IsErrorImage(Texture tex) => 
            (tex && ((tex.name == string.Empty) && ((tex.height == 8) && ((tex.width == 8) && ((tex.filterMode == FilterMode.Bilinear) && ((tex.anisoLevel == 1) && (tex.wrapMode == TextureWrapMode.Repeat))))))) && (tex.mipMapBias == 0f);

        private Texture2D LoadTexture(WWWLoader loader)
        {
            if (loader.WWW.responseHeaders.ContainsKey("Content-Encoding") && loader.WWW.responseHeaders["Content-Encoding"].Equals("gzip"))
            {
                base.Log.WarnFormat("LoadTexture image is gzipped: {0}", loader.URL);
                Texture2D textured = new Texture2D(2, 2);
                return (!textured.LoadImage(DecompressGzip(loader.Bytes, loader.WWW.bytesDownloaded * 2), true) ? null : textured);
            }
            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false, false);
            loader.WWW.LoadImageIntoTexture(tex);
            return tex;
        }

        private bool NeedHideNewsItem(NewsItemNode newsItem)
        {
            NewsItemFilterEvent eventInstance = new NewsItemFilterEvent();
            base.ScheduleEvent(eventInstance, newsItem);
            return eventInstance.Hide;
        }

        [OnEventFire]
        public void OnClick(ButtonClickEvent e, NewsItemWithUINode newsItem, [JoinAll] UserNode user, [JoinByUser] SingleNode<ClientLocaleComponent> clientLocale)
        {
            CheckForTutorialEvent eventInstance = new CheckForTutorialEvent();
            base.ScheduleEvent(eventInstance, newsItem);
            if (!eventInstance.TutorialIsActive)
            {
                base.Log.InfoFormat("OnClickNewsItem: {0}", newsItem);
                if (!string.IsNullOrEmpty(newsItem.newsItem.Data.InternalUrl))
                {
                    NavigateLinkEvent event3 = new NavigateLinkEvent {
                        Link = newsItem.newsItem.Data.InternalUrl
                    };
                    base.ScheduleEvent(event3, newsItem);
                }
                else
                {
                    string externalUrl = newsItem.newsItem.Data.ExternalUrl;
                    if (!string.IsNullOrEmpty(externalUrl))
                    {
                        Application.OpenURL(externalUrl.Replace(ID, user.Entity.Id.ToString()).Replace(RANK, user.userRank.Rank.ToString()).Replace(UID, user.userUid.Uid).Replace(LOCALE, clientLocale.component.LocaleCode));
                    }
                }
            }
        }

        [OnEventFire]
        public void Register(NodeAddedEvent e, SingleNode<ClientLocaleComponent> locale)
        {
        }

        private void SetAsFirstSiblingIfLessShown(NewsItemNode newsItem, Transform containerTransform, GameObject itemObject)
        {
            if (containerTransform.childCount <= 1)
            {
                newsItem.newsItem.ShowCount++;
            }
            else
            {
                EntityBehaviour behaviour = containerTransform.GetChild(0).GetComponent<EntityBehaviour>();
                if ((behaviour != null) && behaviour.Entity.HasComponent<NewsItemComponent>())
                {
                    NewsItemComponent component = behaviour.Entity.GetComponent<NewsItemComponent>();
                    if (newsItem.newsItem.ShowCount >= component.ShowCount)
                    {
                        newsItem.newsItem.ShowCount = component.ShowCount;
                    }
                    else
                    {
                        base.Log.InfoFormat("Reorder item to first: {0}", newsItem);
                        itemObject.GetComponent<RectTransform>().SetAsFirstSibling();
                        newsItem.newsItem.ShowCount++;
                        component.ShowCount--;
                    }
                }
            }
        }

        private void SetImage(NewsItemWithUINode newsItem, Texture texture)
        {
            newsItem.newsItemUI.ImageContainer.SetRawImage(texture);
            this.ConfigureImage(newsItem);
        }

        [OnEventFire]
        public void SetSale(NodeAddedEvent e, NewsItemWithSaleNode newsItem)
        {
            base.Log.InfoFormat("SetSale: {0}", newsItem.newsItemSaleLabel.Text);
            newsItem.newsItemUI.SaleIconVisible = true;
            newsItem.newsItemUI.SaleIconText = newsItem.newsItemSaleLabel.Text;
        }

        [OnEventFire]
        public void UpdateUI(NewsItemUpdatedEvent e, NewsItemNode newsItem, [JoinAll] NewsContainerNode container)
        {
            base.Log.InfoFormat("Update NewsItem: {0}", newsItem);
            this.DeleteUIIfExists(newsItem, true);
            this.CreateUIIfNeed(newsItem, container);
        }

        [OnEventFire]
        public void UrlLoadComplete(LoadCompleteEvent e, NewsItemWithUINode newsItem)
        {
            WWWLoader loader = (WWWLoader) newsItem.Entity.GetComponent<UrlLoaderComponent>().Loader;
            Texture tex = this.LoadTexture(loader);
            if (IsErrorImage(tex))
            {
                base.Log.ErrorFormat("Image decode failed: {0} bytesDownloaded={1} bytesLength={2}", loader.URL, loader.WWW.bytesDownloaded, loader.WWW.bytes.Length);
            }
            else
            {
                base.Log.InfoFormat("PreviewImage loaded: {0}", loader.URL);
                this.textureCache[loader.URL] = tex;
                this.SetImage(newsItem, tex);
            }
        }

        public class NewsContainerNode : Node
        {
            public NewsContainerComponent newsContainer;
        }

        public class NewsItemNode : Node
        {
            public NewsItemComponent newsItem;
        }

        public class NewsItemWithCetralIconNode : NewsSystem.NewsItemWithUINode
        {
            public NewsItemCentralIconDataComponent newsItemCentralIconData;
        }

        public class NewsItemWithPreviewDataNode : NewsSystem.NewsItemWithUINode
        {
            public NewsItemImageDataComponent newsItemImageData;
        }

        public class NewsItemWithSaleNode : NewsSystem.NewsItemWithUINode
        {
            public NewsItemSaleLabelComponent newsItemSaleLabel;
        }

        public class NewsItemWithUINode : NewsSystem.NewsItemNode
        {
            public NewsItemUIComponent newsItemUI;
            public ButtonMappingComponent buttonMapping;
        }

        public class UserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserUidComponent userUid;
            public UserRankComponent userRank;
        }
    }
}

