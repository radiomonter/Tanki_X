namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class EntityBehaviour : MonoBehaviour
    {
        private const string INVALID_TEMPLATE_TYPE_FORMAT = "Invalid Type {0} on object {1}";
        private readonly List<Component> components = new List<Component>();
        [SerializeField, Obsolete]
        public string template;
        [SerializeField]
        private int templateIdLow;
        [SerializeField]
        private int templateIdHigh;
        public string configPath;
        public bool handleAutomaticaly;

        private void AddComponent(Component component)
        {
            ComponentInstanceDataUpdater.Update((EntityInternal) this.Entity, component, null);
            this.Entity.AddComponent(component);
        }

        private void AddJoinComponent<T>(Platform.Kernel.ECS.ClientEntitySystem.API.Entity key) where T: GroupComponent
        {
            GroupComponent component = GroupRegistry.FindOrCreateGroup<T>(key.Id);
            this.Entity.AddComponent(component);
        }

        private void Awake()
        {
            if (base.GetComponents<EntityBehaviour>().Length > 1)
            {
                throw new GameObjectAlreadyContainsEntityBehaviour(base.gameObject);
            }
        }

        public void BuildEntity(Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity)
        {
            if (this.Entity != null)
            {
                throw new EntityAlreadyExistsException(this.Entity.Name);
            }
            this.Entity = entity;
            this.CollectComponents(base.gameObject, new Action<Component>(this.AddComponent));
        }

        public static void CleanUp(GameObject gameObject)
        {
            foreach (EntityBehaviour behaviour in gameObject.GetComponentsInChildren<EntityBehaviour>())
            {
                behaviour.RemoveUnityComponentsFromEntity();
            }
        }

        private void CollectComponents(GameObject gameObject, Action<Component> handler)
        {
            gameObject.GetComponents(typeof(Component), this.components);
            using (List<Component>.Enumerator enumerator = this.components.GetEnumerator())
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    Component current = enumerator.Current;
                    handler((Component) current);
                    if (this.Entity == null)
                    {
                        return;
                    }
                }
            }
            IEnumerator enumerator2 = gameObject.transform.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    Transform current = (Transform) enumerator2.Current;
                    if (current.GetComponent<EntityBehaviour>() == null)
                    {
                        this.CollectComponents(current.gameObject, handler);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator2 as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        public void CreateEntity()
        {
            if (ClientUnityIntegrationUtils.HasWorkingEngine())
            {
                this.DoCreateEntity(EngineService.Engine);
            }
            else
            {
                DelayedEntityBehaviourActivator activator = FindObjectOfType<DelayedEntityBehaviourActivator>();
                if (activator)
                {
                    activator.DelayedEntityBehaviours.Add(this);
                }
                else
                {
                    object[] args = new object[] { base.name, typeof(DelayedEntityBehaviourActivator).Name };
                    Debug.LogWarningFormat("EntityBehaviour {0} can't be delayed, 'cause {1} is not exists", args);
                }
            }
        }

        public void DestroyEntity()
        {
            EngineService.Engine.DeleteEntity(this.Entity);
            this.Entity = null;
        }

        public void DetachFromEntity()
        {
            if (this.handleAutomaticaly)
            {
                throw new Exception("Couldn't detach entity from entityBehaviour in automatically mode");
            }
            if (this.Entity != null)
            {
                if (((EntityInternal) this.Entity).Alive && ClientUnityIntegrationUtils.HasWorkingEngine())
                {
                    this.RemoveUnityComponentsFromEntity();
                }
                this.Entity = null;
            }
        }

        private void DoCreateEntity(Engine engine)
        {
            long templateId = this.TemplateId;
            Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity = (templateId == 0L) ? engine.CreateEntity(base.name) : engine.CreateEntity(templateId, this.configPath);
            this.BuildEntity(entity);
        }

        public void Join<T>(Platform.Kernel.ECS.ClientEntitySystem.API.Entity key) where T: GroupComponent, new()
        {
            this.AddJoinComponent<T>(key);
            this.JoinChildren<T>(base.gameObject.transform, key);
        }

        private void JoinChildren<T>(Transform transform, Platform.Kernel.ECS.ClientEntitySystem.API.Entity key) where T: GroupComponent, new()
        {
            IEnumerator enumerator = transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    EntityBehaviour component = current.GetComponent<EntityBehaviour>();
                    if (component != null)
                    {
                        component.Join<T>(key);
                        continue;
                    }
                    this.JoinChildren<T>(current, key);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private void OnApplicationQuit()
        {
            this.Entity = null;
        }

        private void OnDestroy()
        {
            if (!this.handleAutomaticaly && ((this.Entity != null) && (((EntityInternal) this.Entity).Alive && ClientUnityIntegrationUtils.HasWorkingEngine())))
            {
                this.RemoveUnityComponentsFromEntity();
            }
        }

        private void OnDisable()
        {
            if (this.handleAutomaticaly && ((this.Entity != null) && (((EntityInternal) this.Entity).Alive && ClientUnityIntegrationUtils.HasWorkingEngine())))
            {
                this.DestroyEntity();
            }
        }

        private void OnEnable()
        {
            if (this.handleAutomaticaly)
            {
                this.CreateEntity();
            }
        }

        private void RemoveComponent(Component component)
        {
            Type type = component.GetType();
            if (this.Entity.HasComponent(type))
            {
                this.Entity.RemoveComponent(type);
            }
        }

        public void RemoveUnityComponentsFromEntity()
        {
            if (this.Entity != null)
            {
                this.CollectComponents(base.gameObject, new Action<Component>(this.RemoveComponent));
            }
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.Impl.GroupRegistry GroupRegistry { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public long TemplateId
        {
            get
            {
                if ((this.templateIdHigh == 0) && ((this.templateIdLow == 0) && !string.IsNullOrEmpty(this.template)))
                {
                    Type type = Type.GetType(this.template);
                    if (type == null)
                    {
                        Debug.LogError($"Invalid Type {type} on object {base.name}", this);
                    }
                    else
                    {
                        this.TemplateId = TemplateRegistry.GetTemplateInfo(type).TemplateId;
                    }
                }
                return ((this.templateIdHigh << 0x20) | ((ulong) this.templateIdLow));
            }
            set
            {
                this.templateIdLow = (int) (((ulong) value) & 0xffffffffUL);
                this.templateIdHigh = (int) (value >> 0x20);
            }
        }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity { get; private set; }
    }
}

