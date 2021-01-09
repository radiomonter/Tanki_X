namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;

    public class AttachToScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void AddAttachComponent(NodeAddedEvent e, DialogNode dialogNode)
        {
            this.AttachChildren(dialogNode.dialogs60.gameObject, dialogNode);
        }

        [OnEventFire]
        public void AddAttachComponent(NodeAddedEvent e, ScreenNode screenNode)
        {
            this.AttachChildren(screenNode.screen.gameObject, screenNode);
        }

        private void AttachChildren(GameObject gameObject, ScreenGroupNode node)
        {
            EntityBehaviour component = gameObject.GetComponent<EntityBehaviour>();
            foreach (EntityBehaviour behaviour2 in gameObject.GetComponentsInChildren<EntityBehaviour>(true))
            {
                if (!behaviour2.Equals(component))
                {
                    if (behaviour2.gameObject.GetComponent<AttachToScreenComponent>() == null)
                    {
                        AttachToScreenComponent component2 = behaviour2.gameObject.AddComponent<AttachToScreenComponent>();
                        component2.JoinEntityBehaviour = component;
                        Entity entity = behaviour2.Entity;
                        if ((entity != null) && behaviour2.handleAutomaticaly)
                        {
                            ((EntityInternal) entity).AddComponent(component2);
                        }
                    }
                    else if (behaviour2.Entity != null)
                    {
                        if (behaviour2.Entity.HasComponent<ScreenGroupComponent>())
                        {
                            behaviour2.Entity.RemoveComponent<ScreenGroupComponent>();
                        }
                        node.screenGroup.Attach(behaviour2.Entity);
                    }
                }
            }
        }

        [OnEventFire]
        public void AttachToScreen(NodeAddedEvent e, SingleNode<AttachToScreenComponent> screenElement, [JoinAll] ScreenNode screen)
        {
            AttachToScreenComponent component = screenElement.component;
            screen.screenGroup.Attach(screenElement.Entity);
        }

        public class DialogNode : AttachToScreenSystem.ScreenGroupNode
        {
            public Dialogs60Component dialogs60;
        }

        public class ScreenGroupNode : Node
        {
            public ScreenGroupComponent screenGroup;
        }

        public class ScreenNode : AttachToScreenSystem.ScreenGroupNode
        {
            public ScreenComponent screen;
            public ActiveScreenComponent activeScreen;
        }
    }
}

