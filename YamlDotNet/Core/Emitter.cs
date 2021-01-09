namespace YamlDotNet.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Core.Tokens;

    public class Emitter : IEmitter
    {
        private const int MinBestIndent = 2;
        private const int MaxBestIndent = 9;
        private const int MaxAliasLength = 0x80;
        private static readonly Regex uriReplacer = new Regex(@"[^0-9A-Za-z_\-;?@=$~\\\)\]/:&+,\.\*\(\[!]", RegexOptions.Singleline | RegexOptions.Compiled);
        private readonly TextWriter output;
        private readonly bool outputUsesUnicodeEncoding;
        private readonly bool isCanonical;
        private readonly int bestIndent;
        private readonly int bestWidth;
        private EmitterState state;
        private readonly Stack<EmitterState> states;
        private readonly Queue<ParsingEvent> events;
        private readonly Stack<int> indents;
        private readonly TagDirectiveCollection tagDirectives;
        private int indent;
        private int flowLevel;
        private bool isMappingContext;
        private bool isSimpleKeyContext;
        private bool isRootContext;
        private int column;
        private bool isWhitespace;
        private bool isIndentation;
        private bool isOpenEnded;
        private bool isDocumentEndWritten;
        private readonly AnchorData anchorData;
        private readonly TagData tagData;
        private readonly ScalarData scalarData;
        [CompilerGenerated]
        private static MatchEvaluator <>f__am$cache0;

        public Emitter(TextWriter output) : this(output, 2)
        {
        }

        public Emitter(TextWriter output, int bestIndent) : this(output, bestIndent, 0x7fffffff)
        {
        }

        public Emitter(TextWriter output, int bestIndent, int bestWidth) : this(output, bestIndent, bestWidth, false)
        {
        }

        public Emitter(TextWriter output, int bestIndent, int bestWidth, bool isCanonical)
        {
            this.states = new Stack<EmitterState>();
            this.events = new Queue<ParsingEvent>();
            this.indents = new Stack<int>();
            this.tagDirectives = new TagDirectiveCollection();
            this.anchorData = new AnchorData();
            this.tagData = new TagData();
            this.scalarData = new ScalarData();
            if ((bestIndent < 2) || (bestIndent > 9))
            {
                object[] args = new object[] { 2, 9 };
                throw new ArgumentOutOfRangeException("bestIndent", string.Format(CultureInfo.InvariantCulture, "The bestIndent parameter must be between {0} and {1}.", args));
            }
            this.bestIndent = bestIndent;
            if (bestWidth <= (bestIndent * 2))
            {
                throw new ArgumentOutOfRangeException("bestWidth", "The bestWidth parameter must be greater than bestIndent * 2.");
            }
            this.bestWidth = bestWidth;
            this.isCanonical = isCanonical;
            this.output = output;
            this.outputUsesUnicodeEncoding = this.IsUnicode(output.Encoding);
        }

        private void AnalyzeAnchor(string anchor, bool isAlias)
        {
            this.anchorData.anchor = anchor;
            this.anchorData.isAlias = isAlias;
        }

        private void AnalyzeEvent(ParsingEvent evt)
        {
            this.anchorData.anchor = null;
            this.tagData.handle = null;
            this.tagData.suffix = null;
            AnchorAlias alias = evt as AnchorAlias;
            if (alias != null)
            {
                this.AnalyzeAnchor(alias.Value, true);
            }
            else
            {
                NodeEvent event2 = evt as NodeEvent;
                if (event2 != null)
                {
                    Scalar scalar = evt as Scalar;
                    if (scalar != null)
                    {
                        this.AnalyzeScalar(scalar);
                    }
                    this.AnalyzeAnchor(event2.Anchor, false);
                    if (!string.IsNullOrEmpty(event2.Tag) && (this.isCanonical || event2.IsCanonical))
                    {
                        this.AnalyzeTag(event2.Tag);
                    }
                }
            }
        }

        private void AnalyzeScalar(Scalar scalar)
        {
            string str = scalar.Value;
            this.scalarData.value = str;
            if (str.Length == 0)
            {
                if (scalar.Tag == "tag:yaml.org,2002:null")
                {
                    this.scalarData.isMultiline = false;
                    this.scalarData.isFlowPlainAllowed = false;
                    this.scalarData.isBlockPlainAllowed = true;
                    this.scalarData.isSingleQuotedAllowed = false;
                    this.scalarData.isBlockAllowed = false;
                }
                else
                {
                    this.scalarData.isMultiline = false;
                    this.scalarData.isFlowPlainAllowed = false;
                    this.scalarData.isBlockPlainAllowed = false;
                    this.scalarData.isSingleQuotedAllowed = true;
                    this.scalarData.isBlockAllowed = false;
                }
            }
            else
            {
                bool flag = false;
                bool flag2 = false;
                if (str.StartsWith("---", StringComparison.Ordinal) || str.StartsWith("...", StringComparison.Ordinal))
                {
                    flag = true;
                    flag2 = true;
                }
                CharacterAnalyzer<StringLookAheadBuffer> analyzer = new CharacterAnalyzer<StringLookAheadBuffer>(new StringLookAheadBuffer(str));
                bool flag3 = true;
                bool flag4 = analyzer.IsWhiteBreakOrZero(1);
                bool flag5 = false;
                bool flag6 = false;
                bool flag7 = false;
                bool flag8 = false;
                bool flag9 = false;
                bool flag10 = false;
                bool flag11 = false;
                bool flag12 = false;
                bool flag13 = false;
                bool flag14 = !this.ValueIsRepresentableInOutputEncoding(str);
                for (bool flag15 = true; !analyzer.EndOfInput; flag15 = false)
                {
                    if (!flag15)
                    {
                        if (analyzer.Check(",?[]{}", 0))
                        {
                            flag = true;
                        }
                        if (analyzer.Check(':', 0))
                        {
                            flag = true;
                            if (flag4)
                            {
                                flag2 = true;
                            }
                        }
                        if (analyzer.Check('#', 0) && flag3)
                        {
                            flag = true;
                            flag2 = true;
                        }
                    }
                    else
                    {
                        if (analyzer.Check("#,[]{}&*!|>\\\"%@`", 0))
                        {
                            flag = true;
                            flag2 = true;
                        }
                        if (analyzer.Check("?:", 0))
                        {
                            flag = true;
                            if (flag4)
                            {
                                flag2 = true;
                            }
                        }
                        if (analyzer.Check('-', 0) && flag4)
                        {
                            flag = true;
                            flag2 = true;
                        }
                    }
                    if (!flag14 && !analyzer.IsPrintable(0))
                    {
                        flag14 = true;
                    }
                    if (analyzer.IsBreak(0))
                    {
                        flag13 = true;
                    }
                    if (analyzer.IsSpace(0))
                    {
                        if (flag15)
                        {
                            flag5 = true;
                        }
                        if (analyzer.Buffer.Position >= (analyzer.Buffer.Length - 1))
                        {
                            flag7 = true;
                        }
                        if (flag12)
                        {
                            flag9 = true;
                        }
                        flag11 = true;
                        flag12 = false;
                    }
                    else if (!analyzer.IsBreak(0))
                    {
                        flag11 = false;
                        flag12 = false;
                    }
                    else
                    {
                        if (flag15)
                        {
                            flag6 = true;
                        }
                        if (analyzer.Buffer.Position >= (analyzer.Buffer.Length - 1))
                        {
                            flag8 = true;
                        }
                        if (flag11)
                        {
                            flag10 = true;
                        }
                        flag11 = false;
                        flag12 = true;
                    }
                    flag3 = analyzer.IsWhiteBreakOrZero(0);
                    analyzer.Skip(1);
                    if (!analyzer.EndOfInput)
                    {
                        flag4 = analyzer.IsWhiteBreakOrZero(1);
                    }
                }
                this.scalarData.isFlowPlainAllowed = true;
                this.scalarData.isBlockPlainAllowed = true;
                this.scalarData.isSingleQuotedAllowed = true;
                this.scalarData.isBlockAllowed = true;
                if (flag5 || (flag6 || (flag7 || flag8)))
                {
                    this.scalarData.isFlowPlainAllowed = false;
                    this.scalarData.isBlockPlainAllowed = false;
                }
                if (flag7)
                {
                    this.scalarData.isBlockAllowed = false;
                }
                if (flag9)
                {
                    this.scalarData.isFlowPlainAllowed = false;
                    this.scalarData.isBlockPlainAllowed = false;
                    this.scalarData.isSingleQuotedAllowed = false;
                }
                if (flag10 || flag14)
                {
                    this.scalarData.isFlowPlainAllowed = false;
                    this.scalarData.isBlockPlainAllowed = false;
                    this.scalarData.isSingleQuotedAllowed = false;
                    this.scalarData.isBlockAllowed = false;
                }
                this.scalarData.isMultiline = flag13;
                if (flag13)
                {
                    this.scalarData.isFlowPlainAllowed = false;
                    this.scalarData.isBlockPlainAllowed = false;
                }
                if (flag)
                {
                    this.scalarData.isFlowPlainAllowed = false;
                }
                if (flag2)
                {
                    this.scalarData.isBlockPlainAllowed = false;
                }
            }
        }

        private void AnalyzeTag(string tag)
        {
            this.tagData.handle = tag;
            foreach (TagDirective directive in this.tagDirectives)
            {
                if (tag.StartsWith(directive.Prefix, StringComparison.Ordinal))
                {
                    this.tagData.handle = directive.Handle;
                    this.tagData.suffix = tag.Substring(directive.Prefix.Length);
                    break;
                }
            }
        }

        private void AnalyzeVersionDirective(VersionDirective versionDirective)
        {
            if ((versionDirective.Version.Major != 1) || (versionDirective.Version.Minor != 1))
            {
                throw new YamlException("Incompatible %YAML directive");
            }
        }

        private void AppendTagDirectiveTo(TagDirective value, bool allowDuplicates, TagDirectiveCollection tagDirectives)
        {
            if (!tagDirectives.Contains(value))
            {
                tagDirectives.Add(value);
            }
            else if (!allowDuplicates)
            {
                throw new YamlException("Duplicate %TAG directive.");
            }
        }

        private bool CheckEmptyDocument()
        {
            bool flag;
            int num = 0;
            using (Queue<ParsingEvent>.Enumerator enumerator = this.events.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ParsingEvent current = enumerator.Current;
                        num++;
                        if (num != 2)
                        {
                            continue;
                        }
                        Scalar scalar = current as Scalar;
                        if (scalar != null)
                        {
                            flag = string.IsNullOrEmpty(scalar.Value);
                            break;
                        }
                    }
                    return false;
                }
            }
            return flag;
        }

        private bool CheckEmptyMapping()
        {
            if (this.events.Count < 2)
            {
                return false;
            }
            FakeList<ParsingEvent> list = new FakeList<ParsingEvent>(this.events);
            return ((list[0] is MappingStart) && (list[1] is MappingEnd));
        }

        private bool CheckEmptySequence()
        {
            if (this.events.Count < 2)
            {
                return false;
            }
            FakeList<ParsingEvent> list = new FakeList<ParsingEvent>(this.events);
            return ((list[0] is SequenceStart) && (list[1] is SequenceEnd));
        }

        private bool CheckSimpleKey()
        {
            int num;
            if (this.events.Count < 1)
            {
                return false;
            }
            switch (this.events.Peek().Type)
            {
                case EventType.Alias:
                    num = this.SafeStringLength(this.anchorData.anchor);
                    break;

                case EventType.Scalar:
                    if (this.scalarData.isMultiline)
                    {
                        return false;
                    }
                    num = ((this.SafeStringLength(this.anchorData.anchor) + this.SafeStringLength(this.tagData.handle)) + this.SafeStringLength(this.tagData.suffix)) + this.SafeStringLength(this.scalarData.value);
                    break;

                case EventType.SequenceStart:
                    if (!this.CheckEmptySequence())
                    {
                        return false;
                    }
                    num = (this.SafeStringLength(this.anchorData.anchor) + this.SafeStringLength(this.tagData.handle)) + this.SafeStringLength(this.tagData.suffix);
                    break;

                case EventType.MappingStart:
                    if (!this.CheckEmptySequence())
                    {
                        return false;
                    }
                    num = (this.SafeStringLength(this.anchorData.anchor) + this.SafeStringLength(this.tagData.handle)) + this.SafeStringLength(this.tagData.suffix);
                    break;

                default:
                    return false;
            }
            return (num <= 0x80);
        }

        public void Emit(ParsingEvent @event)
        {
            this.events.Enqueue(@event);
            while (!this.NeedMoreEvents())
            {
                ParsingEvent evt = this.events.Peek();
                try
                {
                    this.AnalyzeEvent(evt);
                    this.StateMachine(evt);
                }
                finally
                {
                    this.events.Dequeue();
                }
            }
        }

        private void EmitAlias()
        {
            this.ProcessAnchor();
            this.state = this.states.Pop();
        }

        private void EmitBlockMappingKey(ParsingEvent evt, bool isFirst)
        {
            if (isFirst)
            {
                this.IncreaseIndent(false, false);
            }
            if (evt is MappingEnd)
            {
                this.indent = this.indents.Pop();
                this.state = this.states.Pop();
            }
            else
            {
                this.WriteIndent();
                if (this.CheckSimpleKey())
                {
                    this.states.Push(EmitterState.BlockMappingSimpleValue);
                    this.EmitNode(evt, false, true, true);
                }
                else
                {
                    this.WriteIndicator("?", true, false, true);
                    this.states.Push(EmitterState.BlockMappingValue);
                    this.EmitNode(evt, false, true, false);
                }
            }
        }

        private void EmitBlockMappingValue(ParsingEvent evt, bool isSimple)
        {
            if (isSimple)
            {
                this.WriteIndicator(":", false, false, false);
            }
            else
            {
                this.WriteIndent();
                this.WriteIndicator(":", true, false, true);
            }
            this.states.Push(EmitterState.BlockMappingKey);
            this.EmitNode(evt, false, true, false);
        }

        private void EmitBlockSequenceItem(ParsingEvent evt, bool isFirst)
        {
            if (isFirst)
            {
                this.IncreaseIndent(false, this.isMappingContext && !this.isIndentation);
            }
            if (evt is SequenceEnd)
            {
                this.indent = this.indents.Pop();
                this.state = this.states.Pop();
            }
            else
            {
                this.WriteIndent();
                this.WriteIndicator("-", true, false, true);
                this.states.Push(EmitterState.BlockSequenceItem);
                this.EmitNode(evt, false, false, false);
            }
        }

        private void EmitComment(Comment comment)
        {
            if (comment.IsInline)
            {
                this.Write(' ');
            }
            else
            {
                this.WriteBreak('\n');
            }
            this.Write("# ");
            this.Write(comment.Value);
            this.isIndentation = true;
        }

        private void EmitDocumentContent(ParsingEvent evt)
        {
            this.states.Push(EmitterState.DocumentEnd);
            this.EmitNode(evt, true, false, false);
        }

        private void EmitDocumentEnd(ParsingEvent evt)
        {
            DocumentEnd end = evt as DocumentEnd;
            if (end == null)
            {
                throw new YamlException("Expected DOCUMENT-END.");
            }
            this.WriteIndent();
            if (!end.IsImplicit)
            {
                this.WriteIndicator("...", true, false, false);
                this.WriteIndent();
                this.isDocumentEndWritten = true;
            }
            this.state = EmitterState.DocumentStart;
            this.tagDirectives.Clear();
        }

        private void EmitDocumentStart(ParsingEvent evt, bool isFirst)
        {
            DocumentStart start = evt as DocumentStart;
            if (start == null)
            {
                if (!(evt is StreamEnd))
                {
                    throw new YamlException("Expected DOCUMENT-START or STREAM-END");
                }
                if (this.isOpenEnded)
                {
                    this.WriteIndicator("...", true, false, false);
                    this.WriteIndent();
                }
                this.state = EmitterState.StreamEnd;
            }
            else
            {
                bool flag = (start.IsImplicit && isFirst) && !this.isCanonical;
                TagDirectiveCollection tagDirectives = this.NonDefaultTagsAmong(start.Tags);
                if (!isFirst && (!this.isDocumentEndWritten && ((start.Version != null) || (tagDirectives.Count > 0))))
                {
                    this.isDocumentEndWritten = false;
                    this.WriteIndicator("...", true, false, false);
                    this.WriteIndent();
                }
                if (start.Version != null)
                {
                    this.AnalyzeVersionDirective(start.Version);
                    flag = false;
                    this.WriteIndicator("%YAML", true, false, false);
                    object[] args = new object[] { 1, 1 };
                    this.WriteIndicator(string.Format(CultureInfo.InvariantCulture, "{0}.{1}", args), true, false, false);
                    this.WriteIndent();
                }
                foreach (TagDirective directive in tagDirectives)
                {
                    this.AppendTagDirectiveTo(directive, false, this.tagDirectives);
                }
                TagDirective[] defaultTagDirectives = Constants.DefaultTagDirectives;
                int index = 0;
                while (true)
                {
                    if (index >= defaultTagDirectives.Length)
                    {
                        if (tagDirectives.Count > 0)
                        {
                            flag = false;
                            TagDirective[] directiveArray2 = Constants.DefaultTagDirectives;
                            int num2 = 0;
                            while (true)
                            {
                                if (num2 >= directiveArray2.Length)
                                {
                                    foreach (TagDirective directive4 in tagDirectives)
                                    {
                                        this.WriteIndicator("%TAG", true, false, false);
                                        this.WriteTagHandle(directive4.Handle);
                                        this.WriteTagContent(directive4.Prefix, true);
                                        this.WriteIndent();
                                    }
                                    break;
                                }
                                TagDirective directive3 = directiveArray2[num2];
                                this.AppendTagDirectiveTo(directive3, true, tagDirectives);
                                num2++;
                            }
                        }
                        if (this.CheckEmptyDocument())
                        {
                            flag = false;
                        }
                        if (!flag)
                        {
                            this.WriteIndent();
                            this.WriteIndicator("---", true, false, false);
                            if (this.isCanonical)
                            {
                                this.WriteIndent();
                            }
                        }
                        this.state = EmitterState.DocumentContent;
                        break;
                    }
                    TagDirective directive2 = defaultTagDirectives[index];
                    this.AppendTagDirectiveTo(directive2, true, this.tagDirectives);
                    index++;
                }
            }
        }

        private void EmitFlowMappingKey(ParsingEvent evt, bool isFirst)
        {
            if (isFirst)
            {
                this.WriteIndicator("{", true, true, false);
                this.IncreaseIndent(true, false);
                this.flowLevel++;
            }
            if (evt is MappingEnd)
            {
                this.flowLevel--;
                this.indent = this.indents.Pop();
                if (this.isCanonical && !isFirst)
                {
                    this.WriteIndicator(",", false, false, false);
                    this.WriteIndent();
                }
                this.WriteIndicator("}", false, false, false);
                this.state = this.states.Pop();
            }
            else
            {
                if (!isFirst)
                {
                    this.WriteIndicator(",", false, false, false);
                }
                if (this.isCanonical || (this.column > this.bestWidth))
                {
                    this.WriteIndent();
                }
                if (!this.isCanonical && this.CheckSimpleKey())
                {
                    this.states.Push(EmitterState.FlowMappingSimpleValue);
                    this.EmitNode(evt, false, true, true);
                }
                else
                {
                    this.WriteIndicator("?", true, false, false);
                    this.states.Push(EmitterState.FlowMappingValue);
                    this.EmitNode(evt, false, true, false);
                }
            }
        }

        private void EmitFlowMappingValue(ParsingEvent evt, bool isSimple)
        {
            if (isSimple)
            {
                this.WriteIndicator(":", false, false, false);
            }
            else
            {
                if (this.isCanonical || (this.column > this.bestWidth))
                {
                    this.WriteIndent();
                }
                this.WriteIndicator(":", true, false, false);
            }
            this.states.Push(EmitterState.FlowMappingKey);
            this.EmitNode(evt, false, true, false);
        }

        private void EmitFlowSequenceItem(ParsingEvent evt, bool isFirst)
        {
            if (isFirst)
            {
                this.WriteIndicator("[", true, true, false);
                this.IncreaseIndent(true, false);
                this.flowLevel++;
            }
            if (evt is SequenceEnd)
            {
                this.flowLevel--;
                this.indent = this.indents.Pop();
                if (this.isCanonical && !isFirst)
                {
                    this.WriteIndicator(",", false, false, false);
                    this.WriteIndent();
                }
                this.WriteIndicator("]", false, false, false);
                this.state = this.states.Pop();
            }
            else
            {
                if (!isFirst)
                {
                    this.WriteIndicator(",", false, false, false);
                }
                if (this.isCanonical || (this.column > this.bestWidth))
                {
                    this.WriteIndent();
                }
                this.states.Push(EmitterState.FlowSequenceItem);
                this.EmitNode(evt, false, false, false);
            }
        }

        private void EmitMappingStart(ParsingEvent evt)
        {
            this.ProcessAnchor();
            this.ProcessTag();
            MappingStart start = (MappingStart) evt;
            this.state = ((this.flowLevel != 0) || (this.isCanonical || ((start.Style == MappingStyle.Flow) || this.CheckEmptyMapping()))) ? EmitterState.FlowMappingFirstKey : EmitterState.BlockMappingFirstKey;
        }

        private void EmitNode(ParsingEvent evt, bool isRoot, bool isMapping, bool isSimpleKey)
        {
            this.isRootContext = isRoot;
            this.isMappingContext = isMapping;
            this.isSimpleKeyContext = isSimpleKey;
            switch (evt.Type)
            {
                case EventType.Alias:
                    this.EmitAlias();
                    break;

                case EventType.Scalar:
                    this.EmitScalar(evt);
                    break;

                case EventType.SequenceStart:
                    this.EmitSequenceStart(evt);
                    break;

                case EventType.MappingStart:
                    this.EmitMappingStart(evt);
                    break;

                default:
                    throw new YamlException($"Expected SCALAR, SEQUENCE-START, MAPPING-START, or ALIAS, got {evt.Type}");
            }
        }

        private void EmitScalar(ParsingEvent evt)
        {
            this.SelectScalarStyle(evt);
            this.ProcessAnchor();
            this.ProcessTag();
            this.IncreaseIndent(true, false);
            this.ProcessScalar();
            this.indent = this.indents.Pop();
            this.state = this.states.Pop();
        }

        private void EmitSequenceStart(ParsingEvent evt)
        {
            this.ProcessAnchor();
            this.ProcessTag();
            SequenceStart start = (SequenceStart) evt;
            this.state = ((this.flowLevel != 0) || (this.isCanonical || ((start.Style == SequenceStyle.Flow) || this.CheckEmptySequence()))) ? EmitterState.FlowSequenceFirstItem : EmitterState.BlockSequenceFirstItem;
        }

        private void EmitStreamStart(ParsingEvent evt)
        {
            if (!(evt is StreamStart))
            {
                throw new ArgumentException("Expected STREAM-START.", "evt");
            }
            this.indent = -1;
            this.column = 0;
            this.isWhitespace = true;
            this.isIndentation = true;
            this.state = EmitterState.FirstDocumentStart;
        }

        private void IncreaseIndent(bool isFlow, bool isIndentless)
        {
            this.indents.Push(this.indent);
            if (this.indent < 0)
            {
                this.indent = !isFlow ? 0 : this.bestIndent;
            }
            else if (!isIndentless)
            {
                this.indent += this.bestIndent;
            }
        }

        private static bool IsBlank(char character) => 
            (character == ' ') || (character == '\t');

        private static bool IsBreak(char character, out char breakChar)
        {
            switch (character)
            {
                case '\n':
                case '\r':
                    break;

                default:
                    if ((character == '\u2028') || (character == '\u2029'))
                    {
                        breakChar = character;
                        return true;
                    }
                    if (character == '\x0085')
                    {
                        break;
                    }
                    breakChar = '\0';
                    return false;
            }
            breakChar = '\n';
            return true;
        }

        private static bool IsPrintable(char character)
        {
            bool flag1;
            if ((((character == '\t') || ((character == '\n') || ((character == '\r') || ((character >= ' ') && (character <= '~'))))) || (character == '\x0085')) || ((character >= '\x00a0') && (character <= 0xd7ff)))
            {
                flag1 = true;
            }
            else
            {
                flag1 = (character >= 0xe000) && (character <= 0xfffd);
            }
            return flag1;
        }

        private static bool IsSpace(char character) => 
            character == ' ';

        private bool IsUnicode(Encoding encoding) => 
            ((encoding is UTF8Encoding) || ((encoding is UnicodeEncoding) || (encoding is UTF7Encoding))) || (encoding is UTF8Encoding);

        private bool NeedMoreEvents()
        {
            int num;
            bool flag;
            if (this.events.Count == 0)
            {
                return true;
            }
            EventType type = this.events.Peek().Type;
            switch (type)
            {
                case EventType.SequenceStart:
                    num = 2;
                    break;

                case EventType.MappingStart:
                    num = 3;
                    break;

                default:
                    if (type != EventType.DocumentStart)
                    {
                        return false;
                    }
                    num = 1;
                    break;
            }
            if (this.events.Count > num)
            {
                return false;
            }
            int num2 = 0;
            using (Queue<ParsingEvent>.Enumerator enumerator = this.events.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ParsingEvent current = enumerator.Current;
                        EventType type2 = current.Type;
                        switch (type2)
                        {
                            case EventType.DocumentStart:
                            case EventType.SequenceStart:
                            case EventType.MappingStart:
                                num2++;
                                break;

                            case EventType.DocumentEnd:
                            case EventType.SequenceEnd:
                            case EventType.MappingEnd:
                                num2--;
                                break;

                            default:
                                break;
                        }
                        if (num2 != 0)
                        {
                            continue;
                        }
                        flag = false;
                    }
                    else
                    {
                        return true;
                    }
                    break;
                }
            }
            return flag;
        }

        private TagDirectiveCollection NonDefaultTagsAmong(IEnumerable<TagDirective> tagCollection)
        {
            TagDirectiveCollection tagDirectives = new TagDirectiveCollection();
            if (tagCollection != null)
            {
                foreach (TagDirective directive in tagCollection)
                {
                    this.AppendTagDirectiveTo(directive, false, tagDirectives);
                }
                foreach (TagDirective directive2 in Constants.DefaultTagDirectives)
                {
                    tagDirectives.Remove(directive2);
                }
            }
            return tagDirectives;
        }

        private void ProcessAnchor()
        {
            if (this.anchorData.anchor != null)
            {
                this.WriteIndicator(!this.anchorData.isAlias ? "&" : "*", true, false, false);
                this.WriteAnchor(this.anchorData.anchor);
            }
        }

        private void ProcessScalar()
        {
            switch (this.scalarData.style)
            {
                case ScalarStyle.Plain:
                    this.WritePlainScalar(this.scalarData.value, !this.isSimpleKeyContext);
                    break;

                case ScalarStyle.SingleQuoted:
                    this.WriteSingleQuotedScalar(this.scalarData.value, !this.isSimpleKeyContext);
                    break;

                case ScalarStyle.DoubleQuoted:
                    this.WriteDoubleQuotedScalar(this.scalarData.value, !this.isSimpleKeyContext);
                    break;

                case ScalarStyle.Literal:
                    this.WriteLiteralScalar(this.scalarData.value);
                    break;

                case ScalarStyle.Folded:
                    this.WriteFoldedScalar(this.scalarData.value);
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private void ProcessTag()
        {
            if ((this.tagData.handle != null) || (this.tagData.suffix != null))
            {
                if (this.tagData.handle == null)
                {
                    this.WriteIndicator("!<", true, false, false);
                    this.WriteTagContent(this.tagData.suffix, false);
                    this.WriteIndicator(">", false, false, false);
                }
                else
                {
                    this.WriteTagHandle(this.tagData.handle);
                    if (this.tagData.suffix != null)
                    {
                        this.WriteTagContent(this.tagData.suffix, false);
                    }
                }
            }
        }

        private int SafeStringLength(string value) => 
            (value != null) ? value.Length : 0;

        private void SelectScalarStyle(ParsingEvent evt)
        {
            Scalar scalar = (Scalar) evt;
            ScalarStyle doubleQuoted = scalar.Style;
            bool flag = (this.tagData.handle == null) && ReferenceEquals(this.tagData.suffix, null);
            if (flag && (!scalar.IsPlainImplicit && !scalar.IsQuotedImplicit))
            {
                throw new YamlException("Neither tag nor isImplicit flags are specified.");
            }
            doubleQuoted ??= (!this.scalarData.isMultiline ? ScalarStyle.Plain : ScalarStyle.Folded);
            if (this.isCanonical)
            {
                doubleQuoted = ScalarStyle.DoubleQuoted;
            }
            if (this.isSimpleKeyContext && this.scalarData.isMultiline)
            {
                doubleQuoted = ScalarStyle.DoubleQuoted;
            }
            if (doubleQuoted == ScalarStyle.Plain)
            {
                if (((this.flowLevel != 0) && !this.scalarData.isFlowPlainAllowed) || ((this.flowLevel == 0) && !this.scalarData.isBlockPlainAllowed))
                {
                    doubleQuoted = ScalarStyle.SingleQuoted;
                }
                if (string.IsNullOrEmpty(this.scalarData.value) && ((this.flowLevel != 0) || this.isSimpleKeyContext))
                {
                    doubleQuoted = ScalarStyle.SingleQuoted;
                }
                if (flag && !scalar.IsPlainImplicit)
                {
                    doubleQuoted = ScalarStyle.SingleQuoted;
                }
            }
            if ((doubleQuoted == ScalarStyle.SingleQuoted) && !this.scalarData.isSingleQuotedAllowed)
            {
                doubleQuoted = ScalarStyle.DoubleQuoted;
            }
            if (((doubleQuoted == ScalarStyle.Literal) || (doubleQuoted == ScalarStyle.Folded)) && (!this.scalarData.isBlockAllowed || ((this.flowLevel != 0) || this.isSimpleKeyContext)))
            {
                doubleQuoted = ScalarStyle.DoubleQuoted;
            }
            this.scalarData.style = doubleQuoted;
        }

        private void StateMachine(ParsingEvent evt)
        {
            Comment comment = evt as Comment;
            if (comment != null)
            {
                this.EmitComment(comment);
            }
            else
            {
                switch (this.state)
                {
                    case EmitterState.StreamStart:
                        this.EmitStreamStart(evt);
                        break;

                    case EmitterState.StreamEnd:
                        throw new YamlException("Expected nothing after STREAM-END");

                    case EmitterState.FirstDocumentStart:
                        this.EmitDocumentStart(evt, true);
                        break;

                    case EmitterState.DocumentStart:
                        this.EmitDocumentStart(evt, false);
                        break;

                    case EmitterState.DocumentContent:
                        this.EmitDocumentContent(evt);
                        break;

                    case EmitterState.DocumentEnd:
                        this.EmitDocumentEnd(evt);
                        break;

                    case EmitterState.FlowSequenceFirstItem:
                        this.EmitFlowSequenceItem(evt, true);
                        break;

                    case EmitterState.FlowSequenceItem:
                        this.EmitFlowSequenceItem(evt, false);
                        break;

                    case EmitterState.FlowMappingFirstKey:
                        this.EmitFlowMappingKey(evt, true);
                        break;

                    case EmitterState.FlowMappingKey:
                        this.EmitFlowMappingKey(evt, false);
                        break;

                    case EmitterState.FlowMappingSimpleValue:
                        this.EmitFlowMappingValue(evt, true);
                        break;

                    case EmitterState.FlowMappingValue:
                        this.EmitFlowMappingValue(evt, false);
                        break;

                    case EmitterState.BlockSequenceFirstItem:
                        this.EmitBlockSequenceItem(evt, true);
                        break;

                    case EmitterState.BlockSequenceItem:
                        this.EmitBlockSequenceItem(evt, false);
                        break;

                    case EmitterState.BlockMappingFirstKey:
                        this.EmitBlockMappingKey(evt, true);
                        break;

                    case EmitterState.BlockMappingKey:
                        this.EmitBlockMappingKey(evt, false);
                        break;

                    case EmitterState.BlockMappingSimpleValue:
                        this.EmitBlockMappingValue(evt, true);
                        break;

                    case EmitterState.BlockMappingValue:
                        this.EmitBlockMappingValue(evt, false);
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private string UrlEncode(string text)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (Match match) {
                    StringBuilder builder = new StringBuilder();
                    foreach (byte num in Encoding.UTF8.GetBytes(match.Value))
                    {
                        builder.AppendFormat("%{0:X02}", num);
                    }
                    return builder.ToString();
                };
            }
            return uriReplacer.Replace(text, <>f__am$cache0);
        }

        private bool ValueIsRepresentableInOutputEncoding(string value)
        {
            if (this.outputUsesUnicodeEncoding)
            {
                return true;
            }
            try
            {
                byte[] bytes = this.output.Encoding.GetBytes(value);
                return this.output.Encoding.GetString(bytes, 0, bytes.Length).Equals(value);
            }
            catch (EncoderFallbackException)
            {
                return false;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        private void Write(char value)
        {
            this.output.Write(value);
            this.column++;
        }

        private void Write(string value)
        {
            this.output.Write(value);
            this.column += value.Length;
        }

        private void WriteAnchor(string value)
        {
            this.Write(value);
            this.isWhitespace = false;
            this.isIndentation = false;
        }

        private void WriteBlockScalarHints(string value)
        {
            CharacterAnalyzer<StringLookAheadBuffer> analyzer = new CharacterAnalyzer<StringLookAheadBuffer>(new StringLookAheadBuffer(value));
            if (analyzer.IsSpace(0) || analyzer.IsBreak(0))
            {
                object[] args = new object[] { this.bestIndent };
                string str = string.Format(CultureInfo.InvariantCulture, "{0}\0", args);
                this.WriteIndicator(str, false, false, false);
            }
            this.isOpenEnded = false;
            string indicator = null;
            if ((value.Length == 0) || !analyzer.IsBreak(value.Length - 1))
            {
                indicator = "-";
            }
            else if ((value.Length >= 2) && analyzer.IsBreak(value.Length - 2))
            {
                indicator = "+";
                this.isOpenEnded = true;
            }
            if (indicator != null)
            {
                this.WriteIndicator(indicator, false, false, false);
            }
        }

        private void WriteBreak(char breakCharacter = '\n')
        {
            if (breakCharacter == '\n')
            {
                this.output.WriteLine();
            }
            else
            {
                this.output.Write(breakCharacter);
            }
            this.column = 0;
        }

        private void WriteDoubleQuotedScalar(string value, bool allowBreaks)
        {
            this.WriteIndicator("\"", true, false, false);
            bool flag = false;
            for (int i = 0; i < value.Length; i++)
            {
                char ch2;
                char character = value[i];
                if (IsPrintable(character) && (!IsBreak(character, out ch2) && ((character != '"') && (character != '\\'))))
                {
                    if (character != ' ')
                    {
                        this.Write(character);
                        flag = false;
                    }
                    else
                    {
                        if (!allowBreaks || (flag || ((this.column <= this.bestWidth) || ((i <= 0) || ((i + 1) >= value.Length)))))
                        {
                            this.Write(character);
                        }
                        else
                        {
                            this.WriteIndent();
                            if (value[i + 1] == ' ')
                            {
                                this.Write('\\');
                            }
                        }
                        flag = true;
                    }
                }
                else
                {
                    this.Write('\\');
                    switch (character)
                    {
                        case '\a':
                            this.Write('a');
                            break;

                        case '\b':
                            this.Write('b');
                            break;

                        case '\t':
                            this.Write('t');
                            break;

                        case '\n':
                            this.Write('n');
                            break;

                        case '\v':
                            this.Write('v');
                            break;

                        case '\f':
                            this.Write('f');
                            break;

                        case '\r':
                            this.Write('r');
                            break;

                        default:
                            if (character == '\u2028')
                            {
                                this.Write('L');
                            }
                            else if (character == '\u2029')
                            {
                                this.Write('P');
                            }
                            else if (character == '\0')
                            {
                                this.Write('0');
                            }
                            else if (character == '\x001b')
                            {
                                this.Write('e');
                            }
                            else if (character == '"')
                            {
                                this.Write('"');
                            }
                            else if (character == '\\')
                            {
                                this.Write('\\');
                            }
                            else if (character == '\x0085')
                            {
                                this.Write('N');
                            }
                            else if (character == '\x00a0')
                            {
                                this.Write('_');
                            }
                            else
                            {
                                short num2 = (short) character;
                                if (num2 <= 0xff)
                                {
                                    this.Write('x');
                                    this.Write(num2.ToString("X02", CultureInfo.InvariantCulture));
                                }
                                else
                                {
                                    this.Write('u');
                                    this.Write(num2.ToString("X04", CultureInfo.InvariantCulture));
                                }
                            }
                            break;
                    }
                    flag = false;
                }
            }
            this.WriteIndicator("\"", false, false, false);
            this.isWhitespace = false;
            this.isIndentation = false;
        }

        private void WriteFoldedScalar(string value)
        {
            bool flag = true;
            bool flag2 = true;
            this.WriteIndicator(">", true, false, false);
            this.WriteBlockScalarHints(value);
            this.WriteBreak('\n');
            this.isIndentation = true;
            this.isWhitespace = true;
            for (int i = 0; i < value.Length; i++)
            {
                char ch2;
                char character = value[i];
                if (!IsBreak(character, out ch2))
                {
                    if (flag)
                    {
                        this.WriteIndent();
                        flag2 = IsBlank(character);
                    }
                    if (!flag && ((character == ' ') && (((i + 1) < value.Length) && ((value[i + 1] != ' ') && (this.column > this.bestWidth)))))
                    {
                        this.WriteIndent();
                    }
                    else
                    {
                        this.Write(character);
                    }
                    this.isIndentation = false;
                    flag = false;
                }
                else
                {
                    if (!flag && (!flag2 && (character == '\n')))
                    {
                        int num2 = 0;
                        while (true)
                        {
                            char ch3;
                            if (((i + num2) >= value.Length) || !IsBreak(value[i + num2], out ch3))
                            {
                                if (((i + num2) < value.Length) && (!IsBlank(value[i + num2]) && !IsBreak(value[i + num2], out ch3)))
                                {
                                    this.WriteBreak('\n');
                                }
                                break;
                            }
                            num2++;
                        }
                    }
                    this.WriteBreak(ch2);
                    this.isIndentation = true;
                    flag = true;
                }
            }
        }

        private void WriteIndent()
        {
            int num = Math.Max(this.indent, 0);
            if (!this.isIndentation || ((this.column > num) || ((this.column == num) && !this.isWhitespace)))
            {
                this.WriteBreak('\n');
            }
            while (this.column < num)
            {
                this.Write(' ');
            }
            this.isWhitespace = true;
            this.isIndentation = true;
        }

        private void WriteIndicator(string indicator, bool needWhitespace, bool whitespace, bool indentation)
        {
            if (needWhitespace && !this.isWhitespace)
            {
                this.Write(' ');
            }
            this.Write(indicator);
            this.isWhitespace = whitespace;
            this.isIndentation &= indentation;
            this.isOpenEnded = false;
        }

        private void WriteLiteralScalar(string value)
        {
            bool flag = true;
            this.WriteIndicator("|", true, false, false);
            this.WriteBlockScalarHints(value);
            this.WriteBreak('\n');
            this.isIndentation = true;
            this.isWhitespace = true;
            for (int i = 0; i < value.Length; i++)
            {
                char character = value[i];
                if ((character != '\r') || (((i + 1) >= value.Length) || (value[i + 1] != '\n')))
                {
                    char ch2;
                    if (IsBreak(character, out ch2))
                    {
                        this.WriteBreak(ch2);
                        this.isIndentation = true;
                        flag = true;
                    }
                    else
                    {
                        if (flag)
                        {
                            this.WriteIndent();
                        }
                        this.Write(character);
                        this.isIndentation = false;
                        flag = false;
                    }
                }
            }
        }

        private void WritePlainScalar(string value, bool allowBreaks)
        {
            if (!this.isWhitespace)
            {
                this.Write(' ');
            }
            bool flag = false;
            bool flag2 = false;
            for (int i = 0; i < value.Length; i++)
            {
                char character = value[i];
                if (IsSpace(character))
                {
                    if (allowBreaks && (!flag && ((this.column > this.bestWidth) && (((i + 1) < value.Length) && (value[i + 1] != ' ')))))
                    {
                        this.WriteIndent();
                    }
                    else
                    {
                        this.Write(character);
                    }
                    flag = true;
                }
                else
                {
                    char ch2;
                    if (IsBreak(character, out ch2))
                    {
                        if (!flag2 && (character == '\n'))
                        {
                            this.WriteBreak('\n');
                        }
                        this.WriteBreak(ch2);
                        this.isIndentation = true;
                        flag2 = true;
                    }
                    else
                    {
                        if (flag2)
                        {
                            this.WriteIndent();
                        }
                        this.Write(character);
                        this.isIndentation = false;
                        flag = false;
                        flag2 = false;
                    }
                }
            }
            this.isWhitespace = false;
            this.isIndentation = false;
            if (this.isRootContext)
            {
                this.isOpenEnded = true;
            }
        }

        private void WriteSingleQuotedScalar(string value, bool allowBreaks)
        {
            this.WriteIndicator("'", true, false, false);
            bool flag = false;
            bool flag2 = false;
            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                if (ch == ' ')
                {
                    if (allowBreaks && (!flag && ((this.column > this.bestWidth) && ((i != 0) && (((i + 1) < value.Length) && (value[i + 1] != ' '))))))
                    {
                        this.WriteIndent();
                    }
                    else
                    {
                        this.Write(ch);
                    }
                    flag = true;
                }
                else
                {
                    char ch2;
                    if (IsBreak(ch, out ch2))
                    {
                        if (!flag2 && (ch == '\n'))
                        {
                            this.WriteBreak('\n');
                        }
                        this.WriteBreak(ch2);
                        this.isIndentation = true;
                        flag2 = true;
                    }
                    else
                    {
                        if (flag2)
                        {
                            this.WriteIndent();
                        }
                        if (ch == '\'')
                        {
                            this.Write(ch);
                        }
                        this.Write(ch);
                        this.isIndentation = false;
                        flag = false;
                        flag2 = false;
                    }
                }
            }
            this.WriteIndicator("'", false, false, false);
            this.isWhitespace = false;
            this.isIndentation = false;
        }

        private void WriteTagContent(string value, bool needsWhitespace)
        {
            if (needsWhitespace && !this.isWhitespace)
            {
                this.Write(' ');
            }
            this.Write(this.UrlEncode(value));
            this.isWhitespace = false;
            this.isIndentation = false;
        }

        private void WriteTagHandle(string value)
        {
            if (!this.isWhitespace)
            {
                this.Write(' ');
            }
            this.Write(value);
            this.isWhitespace = false;
            this.isIndentation = false;
        }

        private class AnchorData
        {
            public string anchor;
            public bool isAlias;
        }

        private class ScalarData
        {
            public string value;
            public bool isMultiline;
            public bool isFlowPlainAllowed;
            public bool isBlockPlainAllowed;
            public bool isSingleQuotedAllowed;
            public bool isBlockAllowed;
            public ScalarStyle style;
        }

        private class TagData
        {
            public string handle;
            public string suffix;
        }
    }
}

