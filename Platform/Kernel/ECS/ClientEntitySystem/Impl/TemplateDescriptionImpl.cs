namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class TemplateDescriptionImpl : TemplateDescription, HierarchyChangedListener
    {
        private static readonly Type AutoAddedComponentInfoType = typeof(AutoAddedComponentInfo);
        public readonly TemplateRegistry templateRegistry;
        public const string TemplatePostfix = "Template";
        private readonly long templateId;
        private readonly Type templateClass;
        private readonly string templateName;
        private readonly string overrideConfigPath;
        private readonly PathOverrideType pathOverrideType;
        private readonly IDictionary<Type, ComponentDescription> ownComponentDescriptions;
        private readonly ICollection<TemplateDescriptionImpl> parentTemplateDescriptions;
        private readonly Dictionary<Type, ComponentDescription> componentDescriptionsByHierarchy;
        private readonly ICollection<HierarchyChangedListener> hierarchyChangedListeners;
        private volatile bool hierarchyChanged;
        private ICollection<Type> autoAddedComponentTypes;
        [CompilerGenerated]
        private static Comparison<Type> <>f__am$cache0;

        public TemplateDescriptionImpl(TemplateRegistry templateRegistry, long templateId, Type templateClass)
        {
            this.templateRegistry = templateRegistry;
            this.ownComponentDescriptions = new Dictionary<Type, ComponentDescription>();
            this.componentDescriptionsByHierarchy = new Dictionary<Type, ComponentDescription>();
            this.hierarchyChangedListeners = new List<HierarchyChangedListener>();
            this.templateId = templateId;
            this.templateClass = templateClass;
            this.templateName = this.CreateTemplateName(templateClass);
            this.parentTemplateDescriptions = this.GetParentDescriptions(templateClass);
            this.overrideConfigPath = this.GetOverrideConfigPath(templateClass, out this.pathOverrideType);
            foreach (TemplateDescriptionImpl impl in this.parentTemplateDescriptions)
            {
                impl.AddHierarchyChangedListener(this);
            }
        }

        public void AddComponentInfoFromClass(Type templateClass)
        {
            foreach (MethodInfo info in templateClass.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (typeof(Component).IsAssignableFrom(info.ReturnType))
                {
                    this.AddComponentInfoFromMethod(info);
                }
            }
            this.OnHierarchyChanged();
        }

        private void AddComponentInfoFromMethod(MethodInfo method)
        {
            ComponentDescriptionImpl impl = new ComponentDescriptionImpl(method);
            impl.CollectInfo(this.templateRegistry.ComponentInfoBuilders);
            if (this.ownComponentDescriptions.ContainsKey(impl.ComponentType))
            {
                throw new DuplicateComponentOnTemplateException(this, impl.ComponentType);
            }
            this.ownComponentDescriptions[impl.ComponentType] = impl;
        }

        public void AddHierarchyChangedListener(HierarchyChangedListener listener)
        {
            this.hierarchyChangedListeners.Add(listener);
        }

        private string CreateTemplateName(Type templateClass)
        {
            string name = templateClass.Name;
            if (name.EndsWith("Template", StringComparison.Ordinal))
            {
                name = name.Substring(0, name.Length - "Template".Length);
            }
            return name;
        }

        public ICollection<Type> GetAutoAddedComponentTypes()
        {
            if (this.autoAddedComponentTypes == null)
            {
                List<Type> list = new List<Type>();
                foreach (ComponentDescription description in this.ComponentDescriptions)
                {
                    if (description.IsInfoPresent(AutoAddedComponentInfoType))
                    {
                        list.Add(description.ComponentType);
                    }
                }
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = (o1, o2) => o1.FullName.CompareTo(o2.FullName);
                }
                list.Sort(<>f__am$cache0);
                this.autoAddedComponentTypes = list;
            }
            return this.autoAddedComponentTypes;
        }

        public ComponentDescription GetComponentDescription(Type componentClass)
        {
            ComponentDescription description = this.ComponentDescriptionsByHierarchy[componentClass];
            if (description == null)
            {
                throw new ComponentNotFoundInTemplateException(componentClass, this.templateName);
            }
            return description;
        }

        private string GetOverrideConfigPath(Type template, out PathOverrideType pathOverrideType)
        {
            foreach (object obj2 in template.GetCustomAttributes(false))
            {
                OverrideConfigPathAttribute attribute = obj2 as OverrideConfigPathAttribute;
                if (attribute != null)
                {
                    pathOverrideType = attribute.pathOverrideType;
                    return attribute.configPath;
                }
            }
            pathOverrideType = PathOverrideType.ABSOLUTE;
            return string.Empty;
        }

        private ICollection<TemplateDescriptionImpl> GetParentDescriptions(Type templateClass)
        {
            ICollection<TemplateDescriptionImpl> is2 = new List<TemplateDescriptionImpl>();
            foreach (Type type in this.templateRegistry.GetParentTemplateClasses(templateClass))
            {
                is2.Add((TemplateDescriptionImpl) this.templateRegistry.GetTemplateInfo(type));
            }
            return is2;
        }

        public bool IsComponentDescriptionPresent(Type componentClass) => 
            this.ComponentDescriptionsByHierarchy.ContainsKey(componentClass);

        public bool IsOverridedConfigPath() => 
            !string.IsNullOrEmpty(this.overrideConfigPath);

        public void OnHierarchyChanged()
        {
            this.hierarchyChanged = true;
            foreach (HierarchyChangedListener listener in this.hierarchyChangedListeners)
            {
                listener.OnHierarchyChanged();
            }
        }

        public string OverrideConfigPath(string path) => 
            this.IsOverridedConfigPath() ? ((this.pathOverrideType != PathOverrideType.RELATIVE) ? this.overrideConfigPath : (path + this.overrideConfigPath)) : path;

        protected IDictionary<Type, ComponentDescription> ComponentDescriptionsByHierarchy
        {
            get
            {
                if (this.hierarchyChanged)
                {
                    this.componentDescriptionsByHierarchy.Clear();
                    foreach (KeyValuePair<Type, ComponentDescription> pair in this.ownComponentDescriptions)
                    {
                        this.componentDescriptionsByHierarchy[pair.Key] = pair.Value;
                    }
                    foreach (TemplateDescriptionImpl impl in this.parentTemplateDescriptions)
                    {
                        foreach (KeyValuePair<Type, ComponentDescription> pair2 in impl.ComponentDescriptionsByHierarchy)
                        {
                            this.componentDescriptionsByHierarchy[pair2.Key] = pair2.Value;
                        }
                    }
                    this.hierarchyChanged = false;
                }
                return this.componentDescriptionsByHierarchy;
            }
        }

        public ICollection<ComponentDescription> ComponentDescriptions =>
            this.ComponentDescriptionsByHierarchy.Values;

        public long TemplateId =>
            this.templateId;

        public string TemplateName =>
            this.templateName;

        public Type TemplateClass =>
            this.templateClass;
    }
}

