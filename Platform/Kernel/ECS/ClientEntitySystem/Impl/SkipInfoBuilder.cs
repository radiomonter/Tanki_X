namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class SkipInfoBuilder
    {
        public static string GetSkipReasonDetails(Handler handler, ArgumentNode fromArgumentNode, ArgumentNode toArgumentNode, Optional<JoinType> join)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} was skiped, because {1} not found ", handler.Name, toArgumentNode.argument.ClassInstanceDescription.NodeClass.FullName);
            GroupComponent component = null;
            if (join.IsPresent() && (!(join.Get() is JoinAllType) && (fromArgumentNode.entityNodes.Count > 0)))
            {
                Entity entity = fromArgumentNode.entityNodes[0].entity;
                Type type = join.Get().ContextComponent.Get();
                if (entity.HasComponent(type))
                {
                    component = (GroupComponent) entity.GetComponent(type);
                }
            }
            Entity entity2 = null;
            List<Type> components = new List<Type>();
            List<Type> list2 = new List<Type>();
            int num = 0;
            foreach (Entity entity3 in ((EngineServiceImpl) Engine).EntityRegistry.GetAllEntities())
            {
                int num2 = 0;
                List<Type> collection = new List<Type>();
                List<Type> list4 = new List<Type>();
                collection.Clear();
                list4.Clear();
                foreach (Type type2 in toArgumentNode.argument.NodeDescription.Components)
                {
                    if (!entity3.HasComponent(type2))
                    {
                        list4.Add(type2);
                        continue;
                    }
                    Component component2 = entity3.GetComponent(type2);
                    if ((component != null) && ReferenceEquals(component2.GetType(), component.GetType()))
                    {
                        if (!((GroupComponent) component2).Key.Equals(component.Key))
                        {
                            continue;
                        }
                        num2++;
                    }
                    num2++;
                    collection.Add(type2);
                }
                if (num2 > num)
                {
                    num = num2;
                    entity2 = entity3;
                    components = new List<Type>(collection);
                    list2 = new List<Type>(list4);
                }
            }
            if (entity2 != null)
            {
                builder.AppendFormat("\n Best node was {0} , presentComponents {1}, absentComponents {2} ", entity2, EcsToStringUtil.ToString(components), EcsToStringUtil.ToString(list2));
            }
            return builder.ToString();
        }

        [Inject]
        public static EngineService Engine { get; set; }
    }
}

