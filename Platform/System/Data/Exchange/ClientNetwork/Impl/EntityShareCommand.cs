namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class EntityShareCommand : AbstractCommand
    {
        private EntityInternal _entity;
        [CompilerGenerated]
        private static Func<Component, string> <>f__am$cache0;

        private void CreateEntity(Engine engine)
        {
            this._entity = this.GetOrCreateEntity();
            this._entity.Name = !string.IsNullOrEmpty(this.EntityName) ? string.Empty : this.GetNameFromTemplate();
            SharedEntityRegistry.SetShared(this.EntityId);
            this.Components.ForEach<Component>(c => this._entity.AddComponentSilent(c));
        }

        public override void Execute(Engine engine)
        {
            this.CreateEntity(engine);
        }

        private string GetNameFromTemplate()
        {
            if (this.EntityTemplateAccessor.IsPresent())
            {
                TemplateDescription templateDescription = this.EntityTemplateAccessor.Get().TemplateDescription;
                if (templateDescription != null)
                {
                    return templateDescription.TemplateName;
                }
            }
            return string.Empty;
        }

        public EntityInternal GetOrCreateEntity()
        {
            if (SharedEntityRegistry.TryGetEntity(this.EntityId, out this._entity))
            {
                this._entity.TemplateAccessor = this.EntityTemplateAccessor;
            }
            else
            {
                this._entity = SharedEntityRegistry.CreateEntity(this.EntityId, this.EntityTemplateAccessor);
            }
            return this._entity;
        }

        public override string ToString()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = c => c.GetType().Name;
            }
            string str = EcsToStringUtil.EnumerableWithoutTypeToString(this.Components.Select<Component, string>(<>f__am$cache0), ", ");
            return $"EntityShareCommand: EntityId={this.EntityId} Components={str}, Entity={this.Entity}";
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.Impl.SharedEntityRegistry SharedEntityRegistry { get; set; }

        [ProtocolParameterOrder(0)]
        public long EntityId { get; set; }

        [ProtocolParameterOrder(1)]
        public Optional<TemplateAccessor> EntityTemplateAccessor { get; set; }

        [ProtocolParameterOrder(2), ProtocolCollection(false, true)]
        public Component[] Components { get; set; }

        [ProtocolTransient]
        public EntityInternal Entity
        {
            get => 
                this._entity;
            set
            {
                this.EntityId = value.Id;
                this._entity = value;
            }
        }

        [ProtocolTransient]
        public string EntityName { get; set; }
    }
}

