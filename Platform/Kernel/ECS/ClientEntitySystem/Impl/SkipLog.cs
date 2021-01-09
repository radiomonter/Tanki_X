namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class SkipLog
    {
        private readonly ICollection<Entity> contextEntities;
        private readonly Handler handler;

        public SkipLog(ICollection<Entity> contextEntities, Handler handler)
        {
            this.contextEntities = contextEntities;
            this.handler = handler;
        }

        private string BuildLog(ICollection<LogPart> logParts)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.GetLogHeader());
            foreach (LogPart part in logParts)
            {
                Optional<string> skipReason = part.GetSkipReason();
                if (skipReason.IsPresent())
                {
                    builder.Append(skipReason.Get());
                    builder.Append("\n");
                }
            }
            return builder.ToString();
        }

        protected string GetLogHeader() => 
            "\nSkipped: " + EcsToStringUtil.ToString(this.handler) + "\n";

        private static void NewLine(StringBuilder o)
        {
            o.Append("\n\t");
        }

        private void PrintContextEntities(StringBuilder o)
        {
            o.Append("\tContext entities:");
            foreach (Entity entity in this.contextEntities)
            {
                NewLine(o);
                o.Append("\t" + EcsToStringUtil.ToStringWithComponents((EntityInternal) entity));
            }
        }

        private void PrintReason(StringBuilder o)
        {
            NewLine(o);
            List<LogPart> parts = new List<LogPart>();
            ICollection<Entity> contextEntities = this.contextEntities;
            foreach (HandlerArgument argument in this.handler.HandlerArgumentsDescription.HandlerArguments)
            {
                contextEntities = this.PrintReasonForHandlerArgument(argument, contextEntities, parts);
            }
            o.Append(this.BuildLog(parts));
        }

        private ICollection<Entity> PrintReasonForHandlerArgument(HandlerArgument handlerArgument, ICollection<Entity> leftEntities, ICollection<LogPart> parts)
        {
            if (handlerArgument.Context)
            {
                parts.Add(new HandlerArgumentLogPart(handlerArgument, this.contextEntities));
                if (handlerArgument.JoinType.IsPresent())
                {
                    parts.Add(new CheckGroupComponentLogPart(handlerArgument, this.contextEntities));
                }
            }
            else if (handlerArgument.JoinType.IsPresent())
            {
                if (handlerArgument.JoinType.Get() is JoinAllType)
                {
                    ICollection<Entity> entities = Flow.Current.NodeCollector.GetEntities(handlerArgument.NodeDescription);
                    parts.Add(new JoinAllLogPart(handlerArgument, entities));
                    return entities;
                }
                if (handlerArgument.JoinType.Get().ContextComponent.IsPresent())
                {
                    <PrintReasonForHandlerArgument>c__AnonStorey0 storey = new <PrintReasonForHandlerArgument>c__AnonStorey0 {
                        groupComponent = handlerArgument.JoinType.Get().ContextComponent.Get()
                    };
                    HashSet<Entity> entities = new HashSet<Entity>();
                    foreach (Entity entity in leftEntities.Where<Entity>(new Func<Entity, bool>(storey.<>m__0)).ToList<Entity>())
                    {
                        foreach (Entity entity2 in ((GroupComponent) entity.GetComponent(storey.groupComponent)).GetGroupMembers(handlerArgument.NodeDescription))
                        {
                            entities.Add(entity2);
                        }
                    }
                    parts.Add(new HandlerArgumentLogPart(handlerArgument, entities));
                    return entities;
                }
            }
            return leftEntities;
        }

        private void PrintStackTrace(StringBuilder o)
        {
            new StringWriter(o).WriteLine("ECS Stack Trace");
        }

        public override string ToString()
        {
            StringBuilder o = new StringBuilder();
            this.PrintReason(o);
            this.PrintContextEntities(o);
            this.PrintStackTrace(o);
            return o.ToString();
        }

        [CompilerGenerated]
        private sealed class <PrintReasonForHandlerArgument>c__AnonStorey0
        {
            internal Type groupComponent;

            internal bool <>m__0(Entity e) => 
                e.HasComponent(this.groupComponent);
        }
    }
}

