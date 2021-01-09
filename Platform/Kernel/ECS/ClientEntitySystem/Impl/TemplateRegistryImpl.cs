namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;

    public class TemplateRegistryImpl : TemplateRegistry
    {
        private readonly IDictionary<long?, TemplateDescription> templates = new Dictionary<long?, TemplateDescription>();
        private readonly ICollection<ComponentInfoBuilder> builders = new List<ComponentInfoBuilder>();

        public TemplateRegistryImpl()
        {
            this.RegisterCoreInfoBuilders();
        }

        private ICollection<Type> GetDirectInterfaces(Type interf)
        {
            HashSet<Type> set = new HashSet<Type>();
            foreach (Type type in interf.GetInterfaces())
            {
                set.Add(type);
            }
            HashSet<Type> other = new HashSet<Type>();
            foreach (Type type2 in set)
            {
                foreach (Type type3 in type2.GetInterfaces())
                {
                    other.Add(type3);
                }
            }
            set.ExceptWith(other);
            return set;
        }

        public ICollection<Type> GetParentTemplateClasses(Type templateClass)
        {
            IList<Type> list = new List<Type>();
            foreach (Type type in templateClass.GetInterfaces())
            {
                if (typeof(Template).IsAssignableFrom(type) && !ReferenceEquals(type, typeof(Template)))
                {
                    list.Add(type);
                }
            }
            return list;
        }

        public TemplateDescription GetTemplateInfo(long id)
        {
            if (!this.templates.ContainsKey(new long?(id)))
            {
                throw new TemplateNotFoundException(id);
            }
            return this.templates[new long?(id)];
        }

        public TemplateDescription GetTemplateInfo(Type templateClass)
        {
            long uid = SerializationUidUtils.GetUid(templateClass);
            return this.GetTemplateInfo(uid);
        }

        public void Register<T>() where T: Template
        {
            this.Register(typeof(T));
        }

        public void Register(Type templateClass)
        {
            if (templateClass.IsDefined(typeof(TemplatePart), true))
            {
                throw new CannotRegisterTemplatePartAsTemplateException(templateClass);
            }
            long uid = SerializationUidUtils.GetUid(templateClass);
            if (!this.templates.ContainsKey(new long?(uid)))
            {
                foreach (Type type in this.GetParentTemplateClasses(templateClass))
                {
                    this.Register(type);
                }
                TemplateDescriptionImpl impl = new TemplateDescriptionImpl(this, uid, templateClass);
                impl.AddComponentInfoFromClass(templateClass);
                this.templates[new long?(uid)] = impl;
            }
        }

        public void RegisterComponentInfoBuilder(ComponentInfoBuilder componentInfoBuilder)
        {
            this.builders.Add(componentInfoBuilder);
        }

        private void RegisterCoreInfoBuilders()
        {
            this.RegisterComponentInfoBuilder(new AutoAddedComponentInfoBuilder());
            this.RegisterComponentInfoBuilder(new ConfigComponentInfoBuilder());
        }

        public void RegisterPart<T>() where T: Template
        {
            this.RegisterPart(typeof(T));
        }

        public void RegisterPart(Type templatePartClass)
        {
            if (templatePartClass.GetCustomAttributes(typeof(TemplatePart), true).Length == 0)
            {
                throw new MissingTemplatePartAttributeException(templatePartClass);
            }
            ICollection<Type> directInterfaces = this.GetDirectInterfaces(templatePartClass);
            if (directInterfaces.Count != 1)
            {
                throw new TemplatePartShouldExtendSingleTemplateException(templatePartClass);
            }
            Collections.Enumerator<Type> enumerator = Collections.GetEnumerator<Type>(directInterfaces);
            enumerator.MoveNext();
            Type current = enumerator.Current;
            if (!typeof(Template).IsAssignableFrom(current) || ReferenceEquals(typeof(Template), current))
            {
                throw new TemplatePartShouldExtendSingleTemplateException(templatePartClass, current);
            }
            Type templateClass = current;
            ((TemplateDescriptionImpl) this.GetTemplateInfo(templateClass)).AddComponentInfoFromClass(templatePartClass);
        }

        public ICollection<ComponentInfoBuilder> ComponentInfoBuilders =>
            this.builders;
    }
}

