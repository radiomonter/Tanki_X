namespace Tanks.Lobby.ClientControls.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;

    public class CarouselSystem : ECSSystem
    {
        [OnEventFire]
        public void ClearCarouselButtons(NodeRemoveEvent e, SingleNode<CarouselComponent> carousel)
        {
            CarouselComponent component = carousel.component;
            CarouselButtonComponent frontButton = component.FrontButton;
            component.BackButton.DestroyButton();
            frontButton.DestroyButton();
        }

        [OnEventFire]
        public void ClearCarouselItems(NodeRemoveEvent e, SingleNode<CarouselItemCollectionComponent> carousel)
        {
            carousel.component.Items.ForEach(item => base.DeleteEntity(item));
        }

        [OnEventFire]
        public void ClickBack(ButtonClickEvent e, SingleNode<CarouselBackButtonComponent> btn, [JoinByCarousel] ReadyCarouselNode carousel)
        {
            this.MoveCarousel(carousel, -1);
        }

        [OnEventFire]
        public void ClickFront(ButtonClickEvent e, SingleNode<CarouselFrontButtonComponent> btn, [JoinByCarousel] ReadyCarouselNode carousel)
        {
            this.MoveCarousel(carousel, 1);
        }

        [OnEventFire]
        public void InitCarouselButton(NodeAddedEvent e, SingleNode<CarouselBackButtonComponent> button)
        {
            button.Entity.AddComponent(new CarouselGroupComponent(button.component.CarouselEntity));
        }

        [OnEventFire]
        public void InitCarouselButton(NodeAddedEvent e, SingleNode<CarouselFrontButtonComponent> button)
        {
            button.Entity.AddComponent(new CarouselGroupComponent(button.component.CarouselEntity));
        }

        [OnEventFire]
        public void InitCarouselButtons(NodeAddedEvent e, SingleNode<CarouselComponent> carousel)
        {
            CarouselComponent component = carousel.component;
            long key = carousel.Entity.CreateGroup<CarouselGroupComponent>().Key;
            CarouselButtonComponent backButton = component.BackButton;
            CarouselButtonComponent frontButton = component.FrontButton;
            backButton.Build(base.CreateEntity(backButton.name), key);
            frontButton.Build(base.CreateEntity(frontButton.name), key);
        }

        [OnEventFire]
        public void InitCarouselItems(NodeAddedEvent e, CarouselConstructorNode carousel)
        {
            <InitCarouselItems>c__AnonStorey0 storey = new <InitCarouselItems>c__AnonStorey0 {
                $this = this,
                carouselGroup = carousel.carouselGroup
            };
            List<string> collection = carousel.configPathCollection.Collection;
            storey.itemTemplateID = carousel.carousel.ItemTemplateId;
            storey.items = new List<Entity>();
            collection.ForEach(new Action<string>(storey.<>m__0));
            CarouselItemCollectionComponent component = new CarouselItemCollectionComponent {
                Items = storey.items
            };
            carousel.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void InitFirstCarouselItem(NodeAddedEvent e, ReadyCarouselNode carousel)
        {
            carousel.carouselItemCollection.Items[carousel.carouselCurrentItemIndex.Index].AddComponent<CarouselCurrentItemComponent>();
        }

        private void MoveCarousel(ReadyCarouselNode carousel, int dir)
        {
            List<Entity> items = carousel.carouselItemCollection.Items;
            int count = items.Count;
            int num2 = count - 1;
            CarouselCurrentItemIndexComponent carouselCurrentItemIndex = carousel.carouselCurrentItemIndex;
            carouselCurrentItemIndex.Index += dir;
            if (carouselCurrentItemIndex.Index >= count)
            {
                carouselCurrentItemIndex.Index = 0;
            }
            else if (carouselCurrentItemIndex.Index < 0)
            {
                carouselCurrentItemIndex.Index = num2;
            }
            Entity entity = items[carouselCurrentItemIndex.Index];
            Entity[] entities = new Entity[] { carousel.Entity, entity };
            base.NewEvent<CarouselItemBeforeChangeEvent>().AttachAll(entities).Schedule();
        }

        [OnEventFire]
        public void SetCarouselItemIndex(SetCarouselItemIndexEvent e, ReadyCarouselNode carousel)
        {
            int index = e.Index;
            carousel.carouselCurrentItemIndex.Index = index;
            int num = index;
            Entity entity = carousel.carouselItemCollection.Items[num];
            Entity[] entities = new Entity[] { carousel.Entity, entity };
            base.NewEvent<CarouselItemBeforeChangeEvent>().AttachAll(entities).Schedule();
        }

        [OnEventFire]
        public void SwitchCarouselItem(CarouselItemBeforeChangeEvent evt, ReadyCarouselNode carousel, CarouselItemNode item, [JoinByCarousel] CarouselCurrentItemNode currentItem)
        {
            currentItem.Entity.RemoveComponent<CarouselCurrentItemComponent>();
            item.Entity.AddComponent<CarouselCurrentItemComponent>();
            Node[] nodes = new Node[] { carousel, item };
            base.NewEvent<CarouselItemChangedEvent>().AttachAll(nodes).Schedule();
        }

        [OnEventFire]
        public void UpdateCarouselText(NodeAddedEvent e, CarouselCurrentItemNode item, [JoinByCarousel] ReadyCarouselNode carousel)
        {
            carousel.carousel.Text.text = item.carouselItemText.LocalizedCaption;
        }

        [CompilerGenerated]
        private sealed class <InitCarouselItems>c__AnonStorey0
        {
            internal long itemTemplateID;
            internal CarouselGroupComponent carouselGroup;
            internal List<Entity> items;
            internal CarouselSystem $this;

            internal void <>m__0(string itemTemplatePath)
            {
                Entity entity = this.$this.CreateEntity(this.itemTemplateID, itemTemplatePath);
                this.carouselGroup.Attach(entity);
                this.items.Add(entity);
            }
        }

        public class CarouselConstructorNode : Node
        {
            public CarouselComponent carousel;
            public ConfigPathCollectionComponent configPathCollection;
            public CarouselGroupComponent carouselGroup;
        }

        public class CarouselCurrentItemNode : CarouselSystem.CarouselItemNode
        {
            public CarouselCurrentItemComponent carouselCurrentItem;
        }

        public class CarouselItemNode : Node
        {
            public CarouselItemTextComponent carouselItemText;
        }

        public class ReadyCarouselNode : Node
        {
            public CarouselComponent carousel;
            public CarouselItemCollectionComponent carouselItemCollection;
            public CarouselCurrentItemIndexComponent carouselCurrentItemIndex;
        }
    }
}

