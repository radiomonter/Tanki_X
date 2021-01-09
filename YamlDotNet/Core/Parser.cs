namespace YamlDotNet.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Core.Tokens;

    public class Parser : IParser
    {
        private readonly Stack<ParserState> states;
        private readonly TagDirectiveCollection tagDirectives;
        private ParserState state;
        private readonly IScanner scanner;
        private ParsingEvent current;
        private Token currentToken;
        private readonly Queue<ParsingEvent> pendingEvents;

        public Parser(TextReader input) : this(new Scanner(input, true))
        {
        }

        public Parser(IScanner scanner)
        {
            this.states = new Stack<ParserState>();
            this.tagDirectives = new TagDirectiveCollection();
            this.pendingEvents = new Queue<ParsingEvent>();
            this.scanner = scanner;
        }

        private static void AddTagDirectives(TagDirectiveCollection directives, IEnumerable<TagDirective> source)
        {
            foreach (TagDirective directive in source)
            {
                if (!directives.Contains(directive))
                {
                    directives.Add(directive);
                }
            }
        }

        private Token GetCurrentToken()
        {
            if (this.currentToken == null)
            {
                while (this.scanner.MoveNextWithoutConsuming())
                {
                    this.currentToken = this.scanner.Current;
                    Comment currentToken = this.currentToken as Comment;
                    if (currentToken == null)
                    {
                        break;
                    }
                    this.pendingEvents.Enqueue(new Comment(currentToken.Value, currentToken.IsInline, currentToken.Start, currentToken.End));
                }
            }
            return this.currentToken;
        }

        public bool MoveNext()
        {
            if (this.state == ParserState.StreamEnd)
            {
                this.current = null;
                return false;
            }
            if (this.pendingEvents.Count == 0)
            {
                this.pendingEvents.Enqueue(this.StateMachine());
            }
            this.current = this.pendingEvents.Dequeue();
            return true;
        }

        private ParsingEvent ParseBlockMappingKey(bool isFirst)
        {
            if (isFirst)
            {
                this.GetCurrentToken();
                this.Skip();
            }
            if (this.GetCurrentToken() is Key)
            {
                Mark end = this.GetCurrentToken().End;
                this.Skip();
                if (!(this.GetCurrentToken() is Key) && (!(this.GetCurrentToken() is Value) && !(this.GetCurrentToken() is BlockEnd)))
                {
                    this.states.Push(ParserState.BlockMappingValue);
                    return this.ParseNode(true, true);
                }
                this.state = ParserState.BlockMappingValue;
                return ProcessEmptyScalar(end);
            }
            if (!(this.GetCurrentToken() is BlockEnd))
            {
                Token currentToken = this.GetCurrentToken();
                throw new SemanticErrorException(currentToken.Start, currentToken.End, "While parsing a block mapping, did not find expected key.");
            }
            this.state = this.states.Pop();
            ParsingEvent event2 = new MappingEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
            this.Skip();
            return event2;
        }

        private ParsingEvent ParseBlockMappingValue()
        {
            if (!(this.GetCurrentToken() is Value))
            {
                this.state = ParserState.BlockMappingKey;
                return ProcessEmptyScalar(this.GetCurrentToken().Start);
            }
            Mark end = this.GetCurrentToken().End;
            this.Skip();
            if (!(this.GetCurrentToken() is Key) && (!(this.GetCurrentToken() is Value) && !(this.GetCurrentToken() is BlockEnd)))
            {
                this.states.Push(ParserState.BlockMappingKey);
                return this.ParseNode(true, true);
            }
            this.state = ParserState.BlockMappingKey;
            return ProcessEmptyScalar(end);
        }

        private ParsingEvent ParseBlockSequenceEntry(bool isFirst)
        {
            if (isFirst)
            {
                this.GetCurrentToken();
                this.Skip();
            }
            if (this.GetCurrentToken() is BlockEntry)
            {
                Mark end = this.GetCurrentToken().End;
                this.Skip();
                if (!(this.GetCurrentToken() is BlockEntry) && !(this.GetCurrentToken() is BlockEnd))
                {
                    this.states.Push(ParserState.BlockSequenceEntry);
                    return this.ParseNode(true, false);
                }
                this.state = ParserState.BlockSequenceEntry;
                return ProcessEmptyScalar(end);
            }
            if (!(this.GetCurrentToken() is BlockEnd))
            {
                Token currentToken = this.GetCurrentToken();
                throw new SemanticErrorException(currentToken.Start, currentToken.End, "While parsing a block collection, did not find expected '-' indicator.");
            }
            this.state = this.states.Pop();
            ParsingEvent event2 = new SequenceEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
            this.Skip();
            return event2;
        }

        private ParsingEvent ParseDocumentContent()
        {
            if (!(this.GetCurrentToken() is VersionDirective) && (!(this.GetCurrentToken() is TagDirective) && (!(this.GetCurrentToken() is DocumentStart) && (!(this.GetCurrentToken() is DocumentEnd) && !(this.GetCurrentToken() is StreamEnd)))))
            {
                return this.ParseNode(true, false);
            }
            this.state = this.states.Pop();
            return ProcessEmptyScalar(this.scanner.CurrentPosition);
        }

        private ParsingEvent ParseDocumentEnd()
        {
            bool isImplicit = true;
            Mark start = this.GetCurrentToken().Start;
            Mark end = start;
            if (this.GetCurrentToken() is DocumentEnd)
            {
                end = this.GetCurrentToken().End;
                this.Skip();
                isImplicit = false;
            }
            this.state = ParserState.DocumentStart;
            return new DocumentEnd(isImplicit, start, end);
        }

        private ParsingEvent ParseDocumentStart(bool isImplicit)
        {
            if (!isImplicit)
            {
                while (this.GetCurrentToken() is DocumentEnd)
                {
                    this.Skip();
                }
            }
            if (isImplicit && (!(this.GetCurrentToken() is VersionDirective) && (!(this.GetCurrentToken() is TagDirective) && (!(this.GetCurrentToken() is DocumentStart) && !(this.GetCurrentToken() is StreamEnd)))))
            {
                TagDirectiveCollection directives = new TagDirectiveCollection();
                this.ProcessDirectives(directives);
                this.states.Push(ParserState.DocumentEnd);
                this.state = ParserState.BlockNode;
                return new DocumentStart(null, directives, true, this.GetCurrentToken().Start, this.GetCurrentToken().End);
            }
            if (this.GetCurrentToken() is StreamEnd)
            {
                this.state = ParserState.StreamEnd;
                ParsingEvent event3 = new StreamEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
                if (this.scanner.MoveNextWithoutConsuming())
                {
                    throw new InvalidOperationException("The scanner should contain no more tokens.");
                }
                return event3;
            }
            Mark start = this.GetCurrentToken().Start;
            TagDirectiveCollection tags = new TagDirectiveCollection();
            VersionDirective version = this.ProcessDirectives(tags);
            Token currentToken = this.GetCurrentToken();
            if (!(currentToken is DocumentStart))
            {
                throw new SemanticErrorException(currentToken.Start, currentToken.End, "Did not find expected <document start>.");
            }
            this.states.Push(ParserState.DocumentEnd);
            this.state = ParserState.DocumentContent;
            ParsingEvent event2 = new DocumentStart(version, tags, false, start, currentToken.End);
            this.Skip();
            return event2;
        }

        private ParsingEvent ParseFlowMappingKey(bool isFirst)
        {
            if (isFirst)
            {
                this.GetCurrentToken();
                this.Skip();
            }
            if (!(this.GetCurrentToken() is FlowMappingEnd))
            {
                if (!isFirst)
                {
                    switch (this.GetCurrentToken())
                    {
                        case (FlowEntry _):
                            break;

                        default:
                        {
                            Token currentToken = this.GetCurrentToken();
                            throw new SemanticErrorException(currentToken.Start, currentToken.End, "While parsing a flow mapping,  did not find expected ',' or '}'.");
                            break;
                        }
                    }
                    this.Skip();
                }
                if (this.GetCurrentToken() is Key)
                {
                    this.Skip();
                    if (!(this.GetCurrentToken() is Value) && (!(this.GetCurrentToken() is FlowEntry) && !(this.GetCurrentToken() is FlowMappingEnd)))
                    {
                        this.states.Push(ParserState.FlowMappingValue);
                        return this.ParseNode(false, false);
                    }
                    this.state = ParserState.FlowMappingValue;
                    return ProcessEmptyScalar(this.GetCurrentToken().Start);
                }
                if (!(this.GetCurrentToken() is FlowMappingEnd))
                {
                    this.states.Push(ParserState.FlowMappingEmptyValue);
                    return this.ParseNode(false, false);
                }
            }
            this.state = this.states.Pop();
            ParsingEvent event2 = new MappingEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
            this.Skip();
            return event2;
        }

        private ParsingEvent ParseFlowMappingValue(bool isEmpty)
        {
            if (isEmpty)
            {
                this.state = ParserState.FlowMappingKey;
                return ProcessEmptyScalar(this.GetCurrentToken().Start);
            }
            if (this.GetCurrentToken() is Value)
            {
                this.Skip();
                if (!(this.GetCurrentToken() is FlowEntry) && !(this.GetCurrentToken() is FlowMappingEnd))
                {
                    this.states.Push(ParserState.FlowMappingKey);
                    return this.ParseNode(false, false);
                }
            }
            this.state = ParserState.FlowMappingKey;
            return ProcessEmptyScalar(this.GetCurrentToken().Start);
        }

        private ParsingEvent ParseFlowSequenceEntry(bool isFirst)
        {
            ParsingEvent event2;
            if (isFirst)
            {
                this.GetCurrentToken();
                this.Skip();
            }
            if (!(this.GetCurrentToken() is FlowSequenceEnd))
            {
                if (!isFirst)
                {
                    if (!(this.GetCurrentToken() is FlowEntry))
                    {
                        Token currentToken = this.GetCurrentToken();
                        throw new SemanticErrorException(currentToken.Start, currentToken.End, "While parsing a flow sequence, did not find expected ',' or ']'.");
                    }
                    this.Skip();
                }
                if (this.GetCurrentToken() is Key)
                {
                    this.state = ParserState.FlowSequenceEntryMappingKey;
                    event2 = new MappingStart(null, null, true, MappingStyle.Flow);
                    this.Skip();
                    return event2;
                }
                if (!(this.GetCurrentToken() is FlowSequenceEnd))
                {
                    this.states.Push(ParserState.FlowSequenceEntry);
                    return this.ParseNode(false, false);
                }
            }
            this.state = this.states.Pop();
            event2 = new SequenceEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
            this.Skip();
            return event2;
        }

        private ParsingEvent ParseFlowSequenceEntryMappingEnd()
        {
            this.state = ParserState.FlowSequenceEntry;
            return new MappingEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
        }

        private ParsingEvent ParseFlowSequenceEntryMappingKey()
        {
            if (!(this.GetCurrentToken() is Value) && (!(this.GetCurrentToken() is FlowEntry) && !(this.GetCurrentToken() is FlowSequenceEnd)))
            {
                this.states.Push(ParserState.FlowSequenceEntryMappingValue);
                return this.ParseNode(false, false);
            }
            Mark end = this.GetCurrentToken().End;
            this.Skip();
            this.state = ParserState.FlowSequenceEntryMappingValue;
            return ProcessEmptyScalar(end);
        }

        private ParsingEvent ParseFlowSequenceEntryMappingValue()
        {
            if (this.GetCurrentToken() is Value)
            {
                this.Skip();
                if (!(this.GetCurrentToken() is FlowEntry) && !(this.GetCurrentToken() is FlowSequenceEnd))
                {
                    this.states.Push(ParserState.FlowSequenceEntryMappingEnd);
                    return this.ParseNode(false, false);
                }
            }
            this.state = ParserState.FlowSequenceEntryMappingEnd;
            return ProcessEmptyScalar(this.GetCurrentToken().Start);
        }

        private ParsingEvent ParseIndentlessSequenceEntry()
        {
            if (!(this.GetCurrentToken() is BlockEntry))
            {
                this.state = this.states.Pop();
                return new SequenceEnd(this.GetCurrentToken().Start, this.GetCurrentToken().End);
            }
            Mark end = this.GetCurrentToken().End;
            this.Skip();
            if (!(this.GetCurrentToken() is BlockEntry) && (!(this.GetCurrentToken() is Key) && (!(this.GetCurrentToken() is Value) && !(this.GetCurrentToken() is BlockEnd))))
            {
                this.states.Push(ParserState.IndentlessSequenceEntry);
                return this.ParseNode(true, false);
            }
            this.state = ParserState.IndentlessSequenceEntry;
            return ProcessEmptyScalar(end);
        }

        private ParsingEvent ParseNode(bool isBlock, bool isIndentlessSequence)
        {
            AnchorAlias currentToken = this.GetCurrentToken() as AnchorAlias;
            if (currentToken != null)
            {
                this.state = this.states.Pop();
                ParsingEvent event2 = new AnchorAlias(currentToken.Value, currentToken.Start, currentToken.End);
                this.Skip();
                return event2;
            }
            Mark mark = this.GetCurrentToken().Start;
            Anchor anchor = null;
            Tag tag = null;
            while (true)
            {
                if ((anchor == null) && ((anchor = this.GetCurrentToken() as Anchor) != null))
                {
                    this.Skip();
                    continue;
                }
                if ((tag != null) || ((tag = this.GetCurrentToken() as Tag) == null))
                {
                    string suffix = null;
                    if (tag != null)
                    {
                        if (string.IsNullOrEmpty(tag.Handle))
                        {
                            suffix = tag.Suffix;
                        }
                        else
                        {
                            if (!this.tagDirectives.Contains(tag.Handle))
                            {
                                throw new SemanticErrorException(tag.Start, tag.End, "While parsing a node, find undefined tag handle.");
                            }
                            suffix = this.tagDirectives[tag.Handle].Prefix + tag.Suffix;
                        }
                    }
                    if (string.IsNullOrEmpty(suffix))
                    {
                        suffix = null;
                    }
                    string str2 = (anchor == null) ? null : (!string.IsNullOrEmpty(anchor.Value) ? anchor.Value : null);
                    bool isImplicit = string.IsNullOrEmpty(suffix);
                    if (isIndentlessSequence && (this.GetCurrentToken() is BlockEntry))
                    {
                        this.state = ParserState.IndentlessSequenceEntry;
                        return new SequenceStart(str2, suffix, isImplicit, SequenceStyle.Block, mark, this.GetCurrentToken().End);
                    }
                    Scalar scalar = this.GetCurrentToken() as Scalar;
                    if (scalar != null)
                    {
                        bool isPlainImplicit = false;
                        bool isQuotedImplicit = false;
                        if (((scalar.Style == ScalarStyle.Plain) && (suffix == null)) || (suffix == "!"))
                        {
                            isPlainImplicit = true;
                        }
                        else if (suffix == null)
                        {
                            isQuotedImplicit = true;
                        }
                        this.state = this.states.Pop();
                        ParsingEvent event3 = new Scalar(str2, suffix, scalar.Value, scalar.Style, isPlainImplicit, isQuotedImplicit, mark, scalar.End);
                        this.Skip();
                        return event3;
                    }
                    FlowSequenceStart start = this.GetCurrentToken() as FlowSequenceStart;
                    if (start != null)
                    {
                        this.state = ParserState.FlowSequenceFirstEntry;
                        return new SequenceStart(str2, suffix, isImplicit, SequenceStyle.Flow, mark, start.End);
                    }
                    FlowMappingStart start2 = this.GetCurrentToken() as FlowMappingStart;
                    if (start2 != null)
                    {
                        this.state = ParserState.FlowMappingFirstKey;
                        return new MappingStart(str2, suffix, isImplicit, MappingStyle.Flow, mark, start2.End);
                    }
                    if (isBlock)
                    {
                        BlockSequenceStart start3 = this.GetCurrentToken() as BlockSequenceStart;
                        if (start3 != null)
                        {
                            this.state = ParserState.BlockSequenceFirstEntry;
                            return new SequenceStart(str2, suffix, isImplicit, SequenceStyle.Block, mark, start3.End);
                        }
                        if (this.GetCurrentToken() is BlockMappingStart)
                        {
                            this.state = ParserState.BlockMappingFirstKey;
                            return new MappingStart(str2, suffix, isImplicit, MappingStyle.Block, mark, this.GetCurrentToken().End);
                        }
                    }
                    if ((str2 == null) && (tag == null))
                    {
                        Token token = this.GetCurrentToken();
                        throw new SemanticErrorException(token.Start, token.End, "While parsing a node, did not find expected node content.");
                    }
                    this.state = this.states.Pop();
                    return new Scalar(str2, suffix, string.Empty, ScalarStyle.Plain, isImplicit, false, mark, this.GetCurrentToken().End);
                }
                this.Skip();
            }
        }

        private ParsingEvent ParseStreamStart()
        {
            StreamStart currentToken = this.GetCurrentToken() as StreamStart;
            if (currentToken == null)
            {
                Token token = this.GetCurrentToken();
                throw new SemanticErrorException(token.Start, token.End, "Did not find expected <stream-start>.");
            }
            this.Skip();
            this.state = ParserState.ImplicitDocumentStart;
            return new StreamStart(currentToken.Start, currentToken.End);
        }

        private VersionDirective ProcessDirectives(TagDirectiveCollection tags)
        {
            VersionDirective directive = null;
            bool flag = false;
            while (true)
            {
                VersionDirective currentToken = this.GetCurrentToken() as VersionDirective;
                if (currentToken != null)
                {
                    if (directive != null)
                    {
                        throw new SemanticErrorException(currentToken.Start, currentToken.End, "Found duplicate %YAML directive.");
                    }
                    if ((currentToken.Version.Major != 1) || (currentToken.Version.Minor != 1))
                    {
                        throw new SemanticErrorException(currentToken.Start, currentToken.End, "Found incompatible YAML document.");
                    }
                    directive = currentToken;
                    flag = true;
                }
                else
                {
                    TagDirective item = this.GetCurrentToken() as TagDirective;
                    if (item == null)
                    {
                        AddTagDirectives(tags, Constants.DefaultTagDirectives);
                        if (flag)
                        {
                            this.tagDirectives.Clear();
                        }
                        AddTagDirectives(this.tagDirectives, tags);
                        return directive;
                    }
                    if (tags.Contains(item.Handle))
                    {
                        throw new SemanticErrorException(item.Start, item.End, "Found duplicate %TAG directive.");
                    }
                    tags.Add(item);
                    flag = true;
                }
                this.Skip();
            }
        }

        private static ParsingEvent ProcessEmptyScalar(Mark position) => 
            new Scalar(null, null, string.Empty, ScalarStyle.Plain, true, false, position, position);

        private void Skip()
        {
            if (this.currentToken != null)
            {
                this.currentToken = null;
                this.scanner.ConsumeCurrent();
            }
        }

        private ParsingEvent StateMachine()
        {
            switch (this.state)
            {
                case ParserState.StreamStart:
                    return this.ParseStreamStart();

                case ParserState.ImplicitDocumentStart:
                    return this.ParseDocumentStart(true);

                case ParserState.DocumentStart:
                    return this.ParseDocumentStart(false);

                case ParserState.DocumentContent:
                    return this.ParseDocumentContent();

                case ParserState.DocumentEnd:
                    return this.ParseDocumentEnd();

                case ParserState.BlockNode:
                    return this.ParseNode(true, false);

                case ParserState.BlockNodeOrIndentlessSequence:
                    return this.ParseNode(true, true);

                case ParserState.FlowNode:
                    return this.ParseNode(false, false);

                case ParserState.BlockSequenceFirstEntry:
                    return this.ParseBlockSequenceEntry(true);

                case ParserState.BlockSequenceEntry:
                    return this.ParseBlockSequenceEntry(false);

                case ParserState.IndentlessSequenceEntry:
                    return this.ParseIndentlessSequenceEntry();

                case ParserState.BlockMappingFirstKey:
                    return this.ParseBlockMappingKey(true);

                case ParserState.BlockMappingKey:
                    return this.ParseBlockMappingKey(false);

                case ParserState.BlockMappingValue:
                    return this.ParseBlockMappingValue();

                case ParserState.FlowSequenceFirstEntry:
                    return this.ParseFlowSequenceEntry(true);

                case ParserState.FlowSequenceEntry:
                    return this.ParseFlowSequenceEntry(false);

                case ParserState.FlowSequenceEntryMappingKey:
                    return this.ParseFlowSequenceEntryMappingKey();

                case ParserState.FlowSequenceEntryMappingValue:
                    return this.ParseFlowSequenceEntryMappingValue();

                case ParserState.FlowSequenceEntryMappingEnd:
                    return this.ParseFlowSequenceEntryMappingEnd();

                case ParserState.FlowMappingFirstKey:
                    return this.ParseFlowMappingKey(true);

                case ParserState.FlowMappingKey:
                    return this.ParseFlowMappingKey(false);

                case ParserState.FlowMappingValue:
                    return this.ParseFlowMappingValue(false);

                case ParserState.FlowMappingEmptyValue:
                    return this.ParseFlowMappingValue(true);
            }
            throw new InvalidOperationException();
        }

        public ParsingEvent Current =>
            this.current;
    }
}

