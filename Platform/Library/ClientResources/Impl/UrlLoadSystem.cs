namespace Platform.Library.ClientResources.Impl
{
    using Assets.platform.library.ClientResources.Scripts.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class UrlLoadSystem : ECSSystem
    {
        [OnEventComplete]
        public void CheckWWWIsDone(UpdateEvent e, SingleNode<UrlLoaderComponent> loaderNode)
        {
            Loader loader = loaderNode.component.Loader;
            if (loader.IsDone)
            {
                if (!string.IsNullOrEmpty(loader.Error))
                {
                    this.HandleError(loaderNode, loader, $"URL: {loader.URL}, Error: {loader.Error}", loaderNode.component.NoErrorEvent);
                }
                else
                {
                    base.Log.InfoFormat("LoadComplete: {0}", loader.URL);
                    base.ScheduleEvent<LoadCompleteEvent>(loaderNode);
                }
            }
        }

        [OnEventFire]
        public void CreateLoader(NodeAddedEvent e, SingleNode<UrlComponent> node)
        {
            base.Log.InfoFormat("CreateLoader: {0}", node.component);
            node.Entity.AddComponent(new UrlLoaderComponent(this.CreateWWWLoader(node.component), node.component.NoErrorEvent));
        }

        private WWWLoader CreateWWWLoader(UrlComponent urlComponent) => 
            new WWWLoader((!urlComponent.Caching || !DiskCaching.Enabled) ? new WWW(urlComponent.Url) : WWW.LoadFromCacheOrDownload(urlComponent.Url, urlComponent.Hash, urlComponent.CRC));

        private void DisposeLoader(SingleNode<UrlLoaderComponent> node)
        {
            node.component.Loader.Dispose();
            node.Entity.RemoveComponent<UrlLoaderComponent>();
        }

        [OnEventComplete]
        public void DisposeLoader(GameDataLoadErrorEvent e, SingleNode<UrlLoaderComponent> node)
        {
            this.DisposeLoader(node);
        }

        [OnEventComplete]
        public void DisposeLoader(InvalidGameDataErrorEvent e, SingleNode<UrlLoaderComponent> node)
        {
            this.DisposeLoader(node);
        }

        [OnEventComplete]
        public void DisposeLoader(LoadCompleteEvent e, SingleNode<UrlLoaderComponent> node)
        {
            this.DisposeLoader(node);
        }

        [OnEventComplete]
        public void DisposeLoader(NoServerConnectionEvent e, SingleNode<UrlLoaderComponent> node)
        {
            this.DisposeLoader(node);
        }

        [OnEventComplete]
        public void DisposeLoader(ServerDisconnectedEvent e, SingleNode<UrlLoaderComponent> node)
        {
            this.DisposeLoader(node);
        }

        [OnEventFire]
        public void DisposeLoader(DisposeUrlLoadersEvent e, Node node, [JoinAll] ICollection<SingleNode<UrlLoaderComponent>> loaderList)
        {
            foreach (SingleNode<UrlLoaderComponent> node2 in loaderList)
            {
                this.DisposeLoader(node2);
            }
        }

        private void HandleError(SingleNode<UrlLoaderComponent> loaderNode, Loader loader, string errorMessage, bool noErrorEvent)
        {
            bool flag = (loader.Progress > 0f) && (loader.Progress < 1f);
            this.DisposeLoader(loaderNode);
            if (flag)
            {
                this.SheduleErrorEvent<ServerDisconnectedEvent>(loaderNode.Entity, errorMessage, noErrorEvent);
            }
            else
            {
                this.SheduleErrorEvent<NoServerConnectionEvent>(loaderNode.Entity, errorMessage, noErrorEvent);
            }
        }

        private void SheduleErrorEvent<T>(Entity entity, string errorMessage, bool noErrorEvent) where T: Event, new()
        {
            base.Log.Error(errorMessage);
            if (!noErrorEvent)
            {
                base.ScheduleEvent<T>(entity);
            }
        }
    }
}

