namespace YamlDotNet.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using YamlDotNet.Core.Events;

    public sealed class MergingParser : IParser
    {
        private readonly List<ParsingEvent> _allEvents = new List<ParsingEvent>();
        private readonly IParser _innerParser;
        private int _currentIndex = -1;
        [CompilerGenerated]
        private static Func<IEnumerable<ParsingEvent>, IEnumerable<ParsingEvent>> <>f__am$cache0;

        public MergingParser(IParser innerParser)
        {
            this._innerParser = innerParser;
        }

        private IEnumerable<ParsingEvent> GetMappingEvents(string mappingAlias)
        {
            <GetMappingEvents>c__AnonStorey0 storey = new <GetMappingEvents>c__AnonStorey0 {
                mappingAlias = mappingAlias,
                cloner = new ParsingEventCloner(),
                nesting = 0
            };
            return this._allEvents.SkipWhile<ParsingEvent>(new Func<ParsingEvent, bool>(storey.<>m__0)).Skip<ParsingEvent>(1).TakeWhile<ParsingEvent>(new Func<ParsingEvent, bool>(storey.<>m__1)).Select<ParsingEvent, ParsingEvent>(new Func<ParsingEvent, ParsingEvent>(storey.<>m__2)).ToList<ParsingEvent>();
        }

        public bool MoveNext()
        {
            int num;
            Scalar scalar;
            bool flag;
            int num3;
            if (this._currentIndex >= 0)
            {
                goto TR_0002;
            }
            else
            {
                while (true)
                {
                    if (!this._innerParser.MoveNext())
                    {
                        num = this._allEvents.Count - 2;
                        break;
                    }
                    this._allEvents.Add(this._innerParser.Current);
                }
            }
            goto TR_001A;
        TR_0002:
            num3 = this._currentIndex + 1;
            if (num3 >= this._allEvents.Count)
            {
                return false;
            }
            this.Current = this._allEvents[num3];
            this._currentIndex = num3;
            return true;
        TR_0004:
            num--;
            goto TR_001A;
        TR_0006:
            throw new SemanticErrorException(scalar.Start, scalar.End, "Unrecognized merge key pattern");
        TR_001A:
            while (true)
            {
                if (num >= 0)
                {
                    scalar = this._allEvents[num] as Scalar;
                    if ((scalar == null) || (scalar.Value != "<<"))
                    {
                        goto TR_0004;
                    }
                    else
                    {
                        AnchorAlias alias = this._allEvents[num + 1] as AnchorAlias;
                        if (alias == null)
                        {
                            if (!(this._allEvents[num + 1] is SequenceStart))
                            {
                                goto TR_0006;
                            }
                            else
                            {
                                List<IEnumerable<ParsingEvent>> source = new List<IEnumerable<ParsingEvent>>();
                                flag = false;
                                for (int i = num + 2; i < this._allEvents.Count; i++)
                                {
                                    alias = this._allEvents[i] as AnchorAlias;
                                    if (alias != null)
                                    {
                                        source.Add(this.GetMappingEvents(alias.Value));
                                    }
                                    else if (this._allEvents[i] is SequenceEnd)
                                    {
                                        this._allEvents.RemoveRange(num, (i - num) + 1);
                                        if (<>f__am$cache0 == null)
                                        {
                                            <>f__am$cache0 = e => e;
                                        }
                                        this._allEvents.InsertRange(num, source.SelectMany<IEnumerable<ParsingEvent>, ParsingEvent>(<>f__am$cache0));
                                        flag = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this._allEvents.RemoveRange(num, 2);
                            this._allEvents.InsertRange(num, this.GetMappingEvents(alias.Value));
                            goto TR_0004;
                        }
                    }
                }
                else
                {
                    goto TR_0002;
                }
                break;
            }
            if (!flag)
            {
                goto TR_0006;
            }
            goto TR_0004;
        }

        public ParsingEvent Current { get; private set; }

        [CompilerGenerated]
        private sealed class <GetMappingEvents>c__AnonStorey0
        {
            internal string mappingAlias;
            internal int nesting;
            internal MergingParser.ParsingEventCloner cloner;

            internal bool <>m__0(ParsingEvent e)
            {
                MappingStart start = e as MappingStart;
                return ((start == null) || (start.Anchor != this.mappingAlias));
            }

            internal bool <>m__1(ParsingEvent e)
            {
                int num;
                this.nesting = num = this.nesting + e.NestingIncrease;
                return (num >= 0);
            }

            internal ParsingEvent <>m__2(ParsingEvent e) => 
                this.cloner.Clone(e);
        }

        private class ParsingEventCloner : IParsingEventVisitor
        {
            private ParsingEvent clonedEvent;

            public ParsingEvent Clone(ParsingEvent e)
            {
                e.Accept(this);
                return this.clonedEvent;
            }

            void IParsingEventVisitor.Visit(AnchorAlias e)
            {
                this.clonedEvent = new AnchorAlias(e.Value, e.Start, e.End);
            }

            void IParsingEventVisitor.Visit(Comment e)
            {
                throw new NotSupportedException();
            }

            void IParsingEventVisitor.Visit(DocumentEnd e)
            {
                throw new NotSupportedException();
            }

            void IParsingEventVisitor.Visit(DocumentStart e)
            {
                throw new NotSupportedException();
            }

            void IParsingEventVisitor.Visit(MappingEnd e)
            {
                this.clonedEvent = new MappingEnd(e.Start, e.End);
            }

            void IParsingEventVisitor.Visit(MappingStart e)
            {
                this.clonedEvent = new MappingStart(null, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);
            }

            void IParsingEventVisitor.Visit(Scalar e)
            {
                this.clonedEvent = new Scalar(null, e.Tag, e.Value, e.Style, e.IsPlainImplicit, e.IsQuotedImplicit, e.Start, e.End);
            }

            void IParsingEventVisitor.Visit(SequenceEnd e)
            {
                this.clonedEvent = new SequenceEnd(e.Start, e.End);
            }

            void IParsingEventVisitor.Visit(SequenceStart e)
            {
                this.clonedEvent = new SequenceStart(null, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);
            }

            void IParsingEventVisitor.Visit(StreamEnd e)
            {
                throw new NotSupportedException();
            }

            void IParsingEventVisitor.Visit(StreamStart e)
            {
                throw new NotSupportedException();
            }
        }
    }
}

