namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class StandardNodeDescription : AbstractNodeDescription
    {
        private static readonly HashSet<Type> PlatformNodeClasses = new HashSet<Type>();

        static StandardNodeDescription()
        {
            PlatformNodeClasses.Add(typeof(NewEntityNode));
            PlatformNodeClasses.Add(typeof(NotNewEntityNode));
            PlatformNodeClasses.Add(typeof(LoadedEntityNode));
            PlatformNodeClasses.Add(typeof(NotLoadedEntityNode));
            PlatformNodeClasses.Add(typeof(DeletedEntityNode));
            PlatformNodeClasses.Add(typeof(NotDeletedEntityNode));
            PlatformNodeClasses.Add(typeof(SharedEntityNode));
            PlatformNodeClasses.Add(typeof(NotSharedEntityNode));
            PlatformNodeClasses.Add(typeof(Node));
        }

        public StandardNodeDescription(Type nodeClass, ICollection<Type> additionalComponents = null) : base(CollectComponents(nodeClass), CollectNotComponents(nodeClass), additionalComponents)
        {
            Check(nodeClass);
        }

        private static void Check(Type nodeClass)
        {
            if (!nodeClass.IsSubclassOf(typeof(AbstractSingleNode)) && !ReferenceEquals(nodeClass, typeof(Node)))
            {
                if (!nodeClass.IsNestedPublic)
                {
                    throw new NodeNotPublicException(nodeClass);
                }
                if (HasNonPublicComponents(nodeClass))
                {
                    throw new NodeWithNonPublicComponentException(nodeClass);
                }
            }
        }

        private static void CheckComponentName(Type nodeClass, Type fieldType, string fieldName)
        {
            string name = typeof(Component).Name;
            string str2 = "Behaviour";
            string str3 = fieldType.Name;
            if (char.ToLower(fieldName[0]) != fieldName[0])
            {
                throw new WrongNodeFieldNameException(nodeClass, fieldType, fieldName);
            }
            if ((((!str3.StartsWith(name) || (fieldName != (char.ToLower(str3[0]) + str3.Substring(1)))) && ((fieldName + name).ToLower() != str3.ToLower())) && ((fieldName + str2).ToLower() != str3.ToLower())) && (fieldName != "marker"))
            {
                throw new WrongNodeFieldNameException(nodeClass, fieldType, fieldName);
            }
        }

        private static ICollection<Type> CollectComponents(Type nodeClass)
        {
            ICollection<Type> is2 = new HashSet<Type>();
            foreach (FieldInfo info in nodeClass.GetFields())
            {
                if (!IsComponent(info))
                {
                    throw new NodeFieldMustBeComponentTypeException($"Node: {nodeClass.FullName}, fieldName: {info.Name}, fieldType: {info.FieldType}");
                }
                is2.Add(info.FieldType);
            }
            return is2;
        }

        private static ICollection<FieldInfo> CollectComponentsField(Type nodeClass)
        {
            ICollection<FieldInfo> is2 = new HashSet<FieldInfo>();
            foreach (FieldInfo info in nodeClass.GetFields())
            {
                if (IsComponent(info))
                {
                    is2.Add(info);
                }
            }
            return is2;
        }

        private static ICollection<Type> CollectNotComponents(Type nodeClass)
        {
            ICollection<Type> is2 = new HashSet<Type>();
            foreach (Not not in nodeClass.GetCustomAttributes(typeof(Not), true))
            {
                is2.Add(not.value);
            }
            return is2;
        }

        private static Type FindDeclaringSystemType(Type nodeClass)
        {
            Type declaringType = nodeClass.DeclaringType;
            while ((declaringType != null) && !typeof(ECSSystem).IsAssignableFrom(declaringType))
            {
                declaringType = declaringType.DeclaringType;
            }
            return declaringType;
        }

        private static bool HasNonPublicComponents(Type nodeClass)
        {
            bool flag;
            using (IEnumerator<FieldInfo> enumerator = nodeClass.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        FieldInfo current = enumerator.Current;
                        if (current.IsPublic)
                        {
                            continue;
                        }
                        if (!IsComponent(current))
                        {
                            continue;
                        }
                        flag = true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                }
            }
            return flag;
        }

        private static bool IsComponent(FieldInfo fieldInfo) => 
            typeof(Component).IsAssignableFrom(fieldInfo.FieldType);
    }
}

