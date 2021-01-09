namespace YamlDotNet.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using YamlDotNet.Core.Tokens;

    [Serializable]
    public class Scanner : IScanner
    {
        private const int MaxVersionNumberLength = 9;
        private const int MaxBufferLength = 8;
        private static readonly IDictionary<char, char> simpleEscapeCodes;
        private readonly Stack<int> indents = new Stack<int>();
        private readonly InsertionQueue<Token> tokens = new InsertionQueue<Token>();
        private readonly Stack<SimpleKey> simpleKeys = new Stack<SimpleKey>();
        private readonly CharacterAnalyzer<LookAheadBuffer> analyzer;
        private Cursor cursor;
        private bool streamStartProduced;
        private bool streamEndProduced;
        private int indent = -1;
        private bool simpleKeyAllowed;
        private int flowLevel;
        private int tokensParsed;
        private bool tokenAvailable;
        private Token previous;

        static Scanner()
        {
            SortedDictionary<char, char> dictionary = new SortedDictionary<char, char> {
                { 
                    '0',
                    '\0'
                },
                { 
                    'a',
                    '\a'
                },
                { 
                    'b',
                    '\b'
                },
                { 
                    't',
                    '\t'
                },
                { 
                    '\t',
                    '\t'
                },
                { 
                    'n',
                    '\n'
                },
                { 
                    'v',
                    '\v'
                },
                { 
                    'f',
                    '\f'
                },
                { 
                    'r',
                    '\r'
                },
                { 
                    'e',
                    '\x001b'
                },
                { 
                    ' ',
                    ' '
                },
                { 
                    '"',
                    '"'
                },
                { 
                    '\'',
                    '\''
                },
                { 
                    '\\',
                    '\\'
                },
                { 
                    'N',
                    '\x0085'
                },
                { 
                    '_',
                    '\x00a0'
                },
                { 
                    'L',
                    '\u2028'
                },
                { 
                    'P',
                    '\u2029'
                }
            };
            simpleEscapeCodes = dictionary;
        }

        public Scanner(TextReader input, bool skipComments = true)
        {
            this.analyzer = new CharacterAnalyzer<LookAheadBuffer>(new LookAheadBuffer(input, 8));
            this.cursor = new Cursor();
            this.SkipComments = skipComments;
        }

        private bool CheckWhiteSpace() => 
            this.analyzer.Check(' ', 0) || (((this.flowLevel > 0) || !this.simpleKeyAllowed) ? this.analyzer.Check('\t', 0) : false);

        public void ConsumeCurrent()
        {
            this.tokensParsed++;
            this.tokenAvailable = false;
            this.previous = this.Current;
            this.Current = null;
        }

        private void DecreaseFlowLevel()
        {
            if (this.flowLevel > 0)
            {
                this.flowLevel--;
                this.simpleKeys.Pop();
            }
        }

        private void FetchAnchor(bool isAlias)
        {
            this.SaveSimpleKey();
            this.simpleKeyAllowed = false;
            this.tokens.Enqueue(this.ScanAnchor(isAlias));
        }

        private void FetchBlockEntry()
        {
            if (this.flowLevel == 0)
            {
                if (!this.simpleKeyAllowed)
                {
                    Mark mark = this.cursor.Mark();
                    throw new SyntaxErrorException(mark, mark, "Block sequence entries are not allowed in this context.");
                }
                this.RollIndent(this.cursor.LineOffset, -1, true, this.cursor.Mark());
            }
            this.RemoveSimpleKey();
            this.simpleKeyAllowed = true;
            Mark start = this.cursor.Mark();
            this.Skip();
            this.tokens.Enqueue(new BlockEntry(start, this.cursor.Mark()));
        }

        private void FetchBlockScalar(bool isLiteral)
        {
            this.RemoveSimpleKey();
            this.simpleKeyAllowed = true;
            this.tokens.Enqueue(this.ScanBlockScalar(isLiteral));
        }

        private void FetchDirective()
        {
            this.UnrollIndent(-1);
            this.RemoveSimpleKey();
            this.simpleKeyAllowed = false;
            Token item = this.ScanDirective();
            this.tokens.Enqueue(item);
        }

        private void FetchDocumentIndicator(bool isStartToken)
        {
            this.UnrollIndent(-1);
            this.RemoveSimpleKey();
            this.simpleKeyAllowed = false;
            Mark start = this.cursor.Mark();
            this.Skip();
            this.Skip();
            this.Skip();
            Token item = !isStartToken ? ((Token) new DocumentEnd(start, start)) : ((Token) new DocumentStart(start, this.cursor.Mark()));
            this.tokens.Enqueue(item);
        }

        private void FetchFlowCollectionEnd(bool isSequenceToken)
        {
            this.RemoveSimpleKey();
            this.DecreaseFlowLevel();
            this.simpleKeyAllowed = false;
            Mark start = this.cursor.Mark();
            this.Skip();
            Token item = !isSequenceToken ? ((Token) new FlowMappingEnd(start, start)) : ((Token) new FlowSequenceEnd(start, start));
            this.tokens.Enqueue(item);
        }

        private void FetchFlowCollectionStart(bool isSequenceToken)
        {
            this.SaveSimpleKey();
            this.IncreaseFlowLevel();
            this.simpleKeyAllowed = true;
            Mark start = this.cursor.Mark();
            this.Skip();
            Token item = !isSequenceToken ? ((Token) new FlowMappingStart(start, start)) : ((Token) new FlowSequenceStart(start, start));
            this.tokens.Enqueue(item);
        }

        private void FetchFlowEntry()
        {
            this.RemoveSimpleKey();
            this.simpleKeyAllowed = true;
            Mark start = this.cursor.Mark();
            this.Skip();
            this.tokens.Enqueue(new FlowEntry(start, this.cursor.Mark()));
        }

        private void FetchFlowScalar(bool isSingleQuoted)
        {
            this.SaveSimpleKey();
            this.simpleKeyAllowed = false;
            this.tokens.Enqueue(this.ScanFlowScalar(isSingleQuoted));
        }

        private void FetchKey()
        {
            if (this.flowLevel == 0)
            {
                if (!this.simpleKeyAllowed)
                {
                    Mark mark = this.cursor.Mark();
                    throw new SyntaxErrorException(mark, mark, "Mapping keys are not allowed in this context.");
                }
                this.RollIndent(this.cursor.LineOffset, -1, false, this.cursor.Mark());
            }
            this.RemoveSimpleKey();
            this.simpleKeyAllowed = this.flowLevel == 0;
            Mark start = this.cursor.Mark();
            this.Skip();
            this.tokens.Enqueue(new Key(start, this.cursor.Mark()));
        }

        private void FetchMoreTokens()
        {
            while (true)
            {
                bool flag = false;
                if (this.tokens.Count == 0)
                {
                    flag = true;
                }
                else
                {
                    this.StaleSimpleKeys();
                    foreach (SimpleKey key in this.simpleKeys)
                    {
                        if (key.IsPossible && (key.TokenNumber == this.tokensParsed))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    this.tokenAvailable = true;
                    return;
                }
                this.FetchNextToken();
            }
        }

        private void FetchNextToken()
        {
            if (!this.streamStartProduced)
            {
                this.FetchStreamStart();
            }
            else
            {
                this.ScanToNextToken();
                this.StaleSimpleKeys();
                this.UnrollIndent(this.cursor.LineOffset);
                this.analyzer.Buffer.Cache(4);
                if (this.analyzer.Buffer.EndOfInput)
                {
                    this.FetchStreamEnd();
                }
                else if ((this.cursor.LineOffset == 0) && this.analyzer.Check('%', 0))
                {
                    this.FetchDirective();
                }
                else if (((this.cursor.LineOffset == 0) && (this.analyzer.Check('-', 0) && (this.analyzer.Check('-', 1) && this.analyzer.Check('-', 2)))) && this.analyzer.IsWhiteBreakOrZero(3))
                {
                    this.FetchDocumentIndicator(true);
                }
                else if (((this.cursor.LineOffset == 0) && (this.analyzer.Check('.', 0) && (this.analyzer.Check('.', 1) && this.analyzer.Check('.', 2)))) && this.analyzer.IsWhiteBreakOrZero(3))
                {
                    this.FetchDocumentIndicator(false);
                }
                else if (this.analyzer.Check('[', 0))
                {
                    this.FetchFlowCollectionStart(true);
                }
                else if (this.analyzer.Check('{', 0))
                {
                    this.FetchFlowCollectionStart(false);
                }
                else if (this.analyzer.Check(']', 0))
                {
                    this.FetchFlowCollectionEnd(true);
                }
                else if (this.analyzer.Check('}', 0))
                {
                    this.FetchFlowCollectionEnd(false);
                }
                else if (this.analyzer.Check(',', 0))
                {
                    this.FetchFlowEntry();
                }
                else if (this.analyzer.Check('-', 0) && this.analyzer.IsWhiteBreakOrZero(1))
                {
                    this.FetchBlockEntry();
                }
                else if (this.analyzer.Check('?', 0) && ((this.flowLevel > 0) || this.analyzer.IsWhiteBreakOrZero(1)))
                {
                    this.FetchKey();
                }
                else if (this.analyzer.Check(':', 0) && ((this.flowLevel > 0) || this.analyzer.IsWhiteBreakOrZero(1)))
                {
                    this.FetchValue();
                }
                else if (this.analyzer.Check('*', 0))
                {
                    this.FetchAnchor(true);
                }
                else if (this.analyzer.Check('&', 0))
                {
                    this.FetchAnchor(false);
                }
                else if (this.analyzer.Check('!', 0))
                {
                    this.FetchTag();
                }
                else if (this.analyzer.Check('|', 0) && (this.flowLevel == 0))
                {
                    this.FetchBlockScalar(true);
                }
                else if (this.analyzer.Check('>', 0) && (this.flowLevel == 0))
                {
                    this.FetchBlockScalar(false);
                }
                else if (this.analyzer.Check('\'', 0))
                {
                    this.FetchFlowScalar(true);
                }
                else if (this.analyzer.Check('"', 0))
                {
                    this.FetchFlowScalar(false);
                }
                else if ((!(this.analyzer.IsWhiteBreakOrZero(0) || this.analyzer.Check("-?:,[]{}#&*!|>'\"%@`", 0)) || (this.analyzer.Check('-', 0) && !this.analyzer.IsWhite(1))) || (((this.flowLevel == 0) && this.analyzer.Check("?:", 0)) && !this.analyzer.IsWhiteBreakOrZero(1)))
                {
                    this.FetchPlainScalar();
                }
                else
                {
                    this.Skip();
                    throw new SyntaxErrorException(this.cursor.Mark(), this.cursor.Mark(), "While scanning for the next token, find character that cannot start any token.");
                }
            }
        }

        private void FetchPlainScalar()
        {
            this.SaveSimpleKey();
            this.simpleKeyAllowed = false;
            this.tokens.Enqueue(this.ScanPlainScalar());
        }

        private void FetchStreamEnd()
        {
            this.cursor.ForceSkipLineAfterNonBreak();
            this.UnrollIndent(-1);
            this.RemoveSimpleKey();
            this.simpleKeyAllowed = false;
            this.streamEndProduced = true;
            Mark start = this.cursor.Mark();
            this.tokens.Enqueue(new StreamEnd(start, start));
        }

        private void FetchStreamStart()
        {
            this.simpleKeys.Push(new SimpleKey());
            this.simpleKeyAllowed = true;
            this.streamStartProduced = true;
            Mark start = this.cursor.Mark();
            this.tokens.Enqueue(new StreamStart(start, start));
        }

        private void FetchTag()
        {
            this.SaveSimpleKey();
            this.simpleKeyAllowed = false;
            this.tokens.Enqueue(this.ScanTag());
        }

        private void FetchValue()
        {
            SimpleKey key = this.simpleKeys.Peek();
            if (key.IsPossible)
            {
                this.tokens.Insert(key.TokenNumber - this.tokensParsed, new Key(key.Mark, key.Mark));
                this.RollIndent(key.LineOffset, key.TokenNumber, false, key.Mark);
                key.IsPossible = false;
                this.simpleKeyAllowed = false;
            }
            else
            {
                if (this.flowLevel == 0)
                {
                    if (!this.simpleKeyAllowed)
                    {
                        Mark mark = this.cursor.Mark();
                        throw new SyntaxErrorException(mark, mark, "Mapping values are not allowed in this context.");
                    }
                    this.RollIndent(this.cursor.LineOffset, -1, false, this.cursor.Mark());
                }
                this.simpleKeyAllowed = this.flowLevel == 0;
            }
            Mark start = this.cursor.Mark();
            this.Skip();
            this.tokens.Enqueue(new Value(start, this.cursor.Mark()));
        }

        private void IncreaseFlowLevel()
        {
            this.simpleKeys.Push(new SimpleKey());
            this.flowLevel++;
        }

        private bool IsDocumentIndicator()
        {
            if ((this.cursor.LineOffset != 0) || !this.analyzer.IsWhiteBreakOrZero(3))
            {
                return false;
            }
            bool flag = (this.analyzer.Check('-', 0) && this.analyzer.Check('-', 1)) && this.analyzer.Check('-', 2);
            bool flag2 = (this.analyzer.Check('.', 0) && this.analyzer.Check('.', 1)) && this.analyzer.Check('.', 2);
            return (flag || flag2);
        }

        public bool MoveNext()
        {
            if (this.Current != null)
            {
                this.ConsumeCurrent();
            }
            return this.MoveNextWithoutConsuming();
        }

        public bool MoveNextWithoutConsuming()
        {
            if (!this.tokenAvailable && !this.streamEndProduced)
            {
                this.FetchMoreTokens();
            }
            if (this.tokens.Count <= 0)
            {
                this.Current = null;
                return false;
            }
            this.Current = this.tokens.Dequeue();
            this.tokenAvailable = false;
            return true;
        }

        private void ProcessComment()
        {
            if (this.analyzer.Check('#', 0))
            {
                Mark start = this.cursor.Mark();
                this.Skip();
                while (true)
                {
                    if (!this.analyzer.IsSpace(0))
                    {
                        StringBuilder builder = new StringBuilder();
                        while (true)
                        {
                            if (this.analyzer.IsBreakOrZero(0))
                            {
                                if (!this.SkipComments)
                                {
                                    bool isInline = ((this.previous != null) && (this.previous.End.Line == start.Line)) && !(this.previous is StreamStart);
                                    this.tokens.Enqueue(new Comment(builder.ToString(), isInline, start, this.cursor.Mark()));
                                }
                                break;
                            }
                            builder.Append(this.ReadCurrentCharacter());
                        }
                        break;
                    }
                    this.Skip();
                }
            }
        }

        private char ReadCurrentCharacter()
        {
            char ch = this.analyzer.Peek(0);
            this.Skip();
            return ch;
        }

        private char ReadLine()
        {
            if (this.analyzer.Check("\r\n\x0085", 0))
            {
                this.SkipLine();
                return '\n';
            }
            char ch = this.analyzer.Peek(0);
            this.SkipLine();
            return ch;
        }

        private void RemoveSimpleKey()
        {
            SimpleKey key = this.simpleKeys.Peek();
            if (key.IsPossible && key.IsRequired)
            {
                throw new SyntaxErrorException(key.Mark, key.Mark, "While scanning a simple key, could not find expected ':'.");
            }
            key.IsPossible = false;
        }

        private void RollIndent(int column, int number, bool isSequence, Mark position)
        {
            if ((this.flowLevel <= 0) && (this.indent < column))
            {
                this.indents.Push(this.indent);
                this.indent = column;
                Token item = !isSequence ? ((Token) new BlockMappingStart(position, position)) : ((Token) new BlockSequenceStart(position, position));
                if (number == -1)
                {
                    this.tokens.Enqueue(item);
                }
                else
                {
                    this.tokens.Insert(number - this.tokensParsed, item);
                }
            }
        }

        private void SaveSimpleKey()
        {
            bool isRequired = (this.flowLevel == 0) && (this.indent == this.cursor.LineOffset);
            if (this.simpleKeyAllowed)
            {
                SimpleKey t = new SimpleKey(true, isRequired, this.tokensParsed + this.tokens.Count, this.cursor);
                this.RemoveSimpleKey();
                this.simpleKeys.Pop();
                this.simpleKeys.Push(t);
            }
        }

        private Token ScanAnchor(bool isAlias)
        {
            Mark start = this.cursor.Mark();
            this.Skip();
            StringBuilder builder = new StringBuilder();
            while (this.analyzer.IsAlphaNumericDashOrUnderscore(0))
            {
                builder.Append(this.ReadCurrentCharacter());
            }
            if ((builder.Length == 0) || (!this.analyzer.IsWhiteBreakOrZero(0) && !this.analyzer.Check("?:,]}%@`", 0)))
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning an anchor or alias, did not find expected alphabetic or numeric character.");
            }
            return (!isAlias ? ((Token) new Anchor(builder.ToString(), start, this.cursor.Mark())) : ((Token) new AnchorAlias(builder.ToString(), start, this.cursor.Mark())));
        }

        private Token ScanBlockScalar(bool isLiteral)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder what = new StringBuilder();
            StringBuilder breaks = new StringBuilder();
            int num = 0;
            int num2 = 0;
            int currentIndent = 0;
            bool flag = false;
            Mark start = this.cursor.Mark();
            this.Skip();
            if (this.analyzer.Check("+-", 0))
            {
                num = !this.analyzer.Check('+', 0) ? -1 : 1;
                this.Skip();
                if (this.analyzer.IsDigit(0))
                {
                    if (this.analyzer.Check('0', 0))
                    {
                        throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a block scalar, find an intendation indicator equal to 0.");
                    }
                    num2 = this.analyzer.AsDigit(0);
                    this.Skip();
                }
            }
            else if (this.analyzer.IsDigit(0))
            {
                if (this.analyzer.Check('0', 0))
                {
                    throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a block scalar, find an intendation indicator equal to 0.");
                }
                num2 = this.analyzer.AsDigit(0);
                this.Skip();
                if (this.analyzer.Check("+-", 0))
                {
                    num = !this.analyzer.Check('+', 0) ? -1 : 1;
                    this.Skip();
                }
            }
            while (this.analyzer.IsWhite(0))
            {
                this.Skip();
            }
            this.ProcessComment();
            if (!this.analyzer.IsBreakOrZero(0))
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a block scalar, did not find expected comment or line break.");
            }
            if (this.analyzer.IsBreak(0))
            {
                this.SkipLine();
            }
            Mark end = this.cursor.Mark();
            if (num2 != 0)
            {
                currentIndent = (this.indent < 0) ? num2 : (this.indent + num2);
            }
            currentIndent = this.ScanBlockScalarBreaks(currentIndent, breaks, start, ref end);
            while ((this.cursor.LineOffset == currentIndent) && !this.analyzer.IsZero(0))
            {
                bool flag2 = this.analyzer.IsWhite(0);
                if (isLiteral || (!StartsWith(what, '\n') || (flag || flag2)))
                {
                    builder.Append(what.ToString());
                    what.Length = 0;
                }
                else
                {
                    if (breaks.Length == 0)
                    {
                        builder.Append(' ');
                    }
                    what.Length = 0;
                }
                builder.Append(breaks.ToString());
                breaks.Length = 0;
                flag = this.analyzer.IsWhite(0);
                while (true)
                {
                    if (this.analyzer.IsBreakOrZero(0))
                    {
                        char ch = this.ReadLine();
                        if (ch != '\0')
                        {
                            what.Append(ch);
                        }
                        currentIndent = this.ScanBlockScalarBreaks(currentIndent, breaks, start, ref end);
                        break;
                    }
                    builder.Append(this.ReadCurrentCharacter());
                }
            }
            if (num != -1)
            {
                builder.Append(what);
            }
            if (num == 1)
            {
                builder.Append(breaks);
            }
            return new Scalar(builder.ToString(), !isLiteral ? ScalarStyle.Folded : ScalarStyle.Literal, start, end);
        }

        private int ScanBlockScalarBreaks(int currentIndent, StringBuilder breaks, Mark start, ref Mark end)
        {
            int lineOffset = 0;
            end = this.cursor.Mark();
            while (true)
            {
                if (((currentIndent == 0) || (this.cursor.LineOffset < currentIndent)) && this.analyzer.IsSpace(0))
                {
                    this.Skip();
                    continue;
                }
                if (this.cursor.LineOffset > lineOffset)
                {
                    lineOffset = this.cursor.LineOffset;
                }
                if (((currentIndent == 0) || (this.cursor.LineOffset < currentIndent)) && this.analyzer.IsTab(0))
                {
                    throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a block scalar, find a tab character where an intendation space is expected.");
                }
                if (!this.analyzer.IsBreak(0))
                {
                    currentIndent ??= Math.Max(lineOffset, Math.Max(this.indent + 1, 1));
                    return currentIndent;
                }
                breaks.Append(this.ReadLine());
                end = this.cursor.Mark();
            }
        }

        private Token ScanDirective()
        {
            Mark start = this.cursor.Mark();
            this.Skip();
            string str = this.ScanDirectiveName(start);
            if (str != null)
            {
                Token token;
                if (str == "YAML")
                {
                    token = this.ScanVersionDirectiveValue(start);
                }
                else if (str == "TAG")
                {
                    token = this.ScanTagDirectiveValue(start);
                }
                else
                {
                    goto TR_0000;
                }
                while (this.analyzer.IsWhite(0))
                {
                    this.Skip();
                }
                this.ProcessComment();
                if (!this.analyzer.IsBreakOrZero(0))
                {
                    throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a directive, did not find expected comment or line break.");
                }
                if (this.analyzer.IsBreak(0))
                {
                    this.SkipLine();
                }
                return token;
            }
        TR_0000:
            throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a directive, find uknown directive name.");
        }

        private string ScanDirectiveName(Mark start)
        {
            StringBuilder builder = new StringBuilder();
            while (this.analyzer.IsAlphaNumericDashOrUnderscore(0))
            {
                builder.Append(this.ReadCurrentCharacter());
            }
            if (builder.Length == 0)
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a directive, could not find expected directive name.");
            }
            if (!this.analyzer.IsWhiteBreakOrZero(0))
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a directive, find unexpected non-alphabetical character.");
            }
            return builder.ToString();
        }

        private Token ScanFlowScalar(bool isSingleQuoted)
        {
            bool flag;
            Mark start = this.cursor.Mark();
            this.Skip();
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            StringBuilder what = new StringBuilder();
            StringBuilder builder4 = new StringBuilder();
            goto TR_0039;
        TR_0014:
            if (this.analyzer.Check(!isSingleQuoted ? '"' : '\'', 0))
            {
                this.Skip();
                return new Scalar(builder.ToString(), !isSingleQuoted ? ScalarStyle.DoubleQuoted : ScalarStyle.SingleQuoted, start, this.cursor.Mark());
            }
            while (true)
            {
                if (!this.analyzer.IsWhite(0) && !this.analyzer.IsBreak(0))
                {
                    if (!flag)
                    {
                        builder.Append(builder2.ToString());
                        builder2.Length = 0;
                    }
                    else
                    {
                        if (!StartsWith(what, '\n'))
                        {
                            builder.Append(what.ToString());
                            builder.Append(builder4.ToString());
                        }
                        else if (builder4.Length == 0)
                        {
                            builder.Append(' ');
                        }
                        else
                        {
                            builder.Append(builder4.ToString());
                        }
                        what.Length = 0;
                        builder4.Length = 0;
                    }
                    break;
                }
                if (this.analyzer.IsWhite(0))
                {
                    if (!flag)
                    {
                        builder2.Append(this.ReadCurrentCharacter());
                        continue;
                    }
                    this.Skip();
                    continue;
                }
                if (flag)
                {
                    builder4.Append(this.ReadLine());
                    continue;
                }
                builder2.Length = 0;
                what.Append(this.ReadLine());
                flag = true;
            }
        TR_0039:
            while (true)
            {
                if (this.IsDocumentIndicator())
                {
                    throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a quoted scalar, find unexpected document indicator.");
                }
                if (this.analyzer.IsZero(0))
                {
                    throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a quoted scalar, find unexpected end of stream.");
                }
                flag = false;
                while (true)
                {
                    if (!this.analyzer.IsWhiteBreakOrZero(0))
                    {
                        if (isSingleQuoted && (this.analyzer.Check('\'', 0) && this.analyzer.Check('\'', 1)))
                        {
                            builder.Append('\'');
                            this.Skip();
                            this.Skip();
                            continue;
                        }
                        if (!this.analyzer.Check(!isSingleQuoted ? '"' : '\'', 0))
                        {
                            if (isSingleQuoted || (!this.analyzer.Check('\\', 0) || !this.analyzer.IsBreak(1)))
                            {
                                if (isSingleQuoted || !this.analyzer.Check('\\', 0))
                                {
                                    builder.Append(this.ReadCurrentCharacter());
                                    continue;
                                }
                                int num = 0;
                                char key = this.analyzer.Peek(1);
                                switch (key)
                                {
                                    case 'u':
                                        num = 4;
                                        break;

                                    case 'x':
                                        num = 2;
                                        break;

                                    default:
                                        if (key == 'U')
                                        {
                                            num = 8;
                                        }
                                        else
                                        {
                                            char ch2;
                                            if (!simpleEscapeCodes.TryGetValue(key, out ch2))
                                            {
                                                throw new SyntaxErrorException(start, this.cursor.Mark(), "While parsing a quoted scalar, find unknown escape character.");
                                            }
                                            builder.Append(ch2);
                                        }
                                        break;
                                }
                                this.Skip();
                                this.Skip();
                                if (num > 0)
                                {
                                    uint num2 = 0;
                                    int offset = 0;
                                    while (true)
                                    {
                                        if (offset < num)
                                        {
                                            if (!this.analyzer.IsHex(offset))
                                            {
                                                throw new SyntaxErrorException(start, this.cursor.Mark(), "While parsing a quoted scalar, did not find expected hexdecimal number.");
                                            }
                                            num2 = (num2 << 4) + ((uint) this.analyzer.AsHex(offset));
                                            offset++;
                                            continue;
                                        }
                                        if ((num2 < 0xd800) || (num2 > 0xdfff))
                                        {
                                            if (num2 <= 0x10ffff)
                                            {
                                                builder.Append((char) num2);
                                                for (int i = 0; i < num; i++)
                                                {
                                                    this.Skip();
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                        break;
                                    }
                                }
                                continue;
                            }
                            else
                            {
                                this.Skip();
                                this.SkipLine();
                                flag = true;
                            }
                        }
                        goto TR_0014;
                    }
                    else
                    {
                        goto TR_0014;
                    }
                    break;
                }
                break;
            }
            throw new SyntaxErrorException(start, this.cursor.Mark(), "While parsing a quoted scalar, find invalid Unicode character escape code.");
        }

        private Token ScanPlainScalar()
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            StringBuilder what = new StringBuilder();
            StringBuilder builder4 = new StringBuilder();
            bool flag = false;
            int num = this.indent + 1;
            Mark start = this.cursor.Mark();
            Mark end = start;
            while (true)
            {
                if (!this.IsDocumentIndicator() && !this.analyzer.Check('#', 0))
                {
                    while (true)
                    {
                        if (!this.analyzer.IsWhiteBreakOrZero(0))
                        {
                            if ((this.flowLevel > 0) && (this.analyzer.Check(':', 0) && !this.analyzer.IsWhiteBreakOrZero(1)))
                            {
                                throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a plain scalar, find unexpected ':'.");
                            }
                            if ((!this.analyzer.Check(':', 0) || !this.analyzer.IsWhiteBreakOrZero(1)) && ((this.flowLevel <= 0) || !this.analyzer.Check(",:?[]{}", 0)))
                            {
                                if (flag || (builder2.Length > 0))
                                {
                                    if (!flag)
                                    {
                                        builder.Append(builder2);
                                        builder2.Length = 0;
                                    }
                                    else
                                    {
                                        if (!StartsWith(what, '\n'))
                                        {
                                            builder.Append(what);
                                            builder.Append(builder4);
                                        }
                                        else if (builder4.Length == 0)
                                        {
                                            builder.Append(' ');
                                        }
                                        else
                                        {
                                            builder.Append(builder4);
                                        }
                                        what.Length = 0;
                                        builder4.Length = 0;
                                        flag = false;
                                    }
                                }
                                builder.Append(this.ReadCurrentCharacter());
                                end = this.cursor.Mark();
                                continue;
                            }
                        }
                        if (this.analyzer.IsWhite(0) || this.analyzer.IsBreak(0))
                        {
                            do
                            {
                                if (this.analyzer.IsWhite(0) || this.analyzer.IsBreak(0))
                                {
                                    if (!this.analyzer.IsWhite(0))
                                    {
                                        if (flag)
                                        {
                                            builder4.Append(this.ReadLine());
                                            continue;
                                        }
                                        builder2.Length = 0;
                                        what.Append(this.ReadLine());
                                        flag = true;
                                        continue;
                                    }
                                    if (flag && ((this.cursor.LineOffset < num) && this.analyzer.IsTab(0)))
                                    {
                                        throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a plain scalar, find a tab character that violate intendation.");
                                    }
                                    if (!flag)
                                    {
                                        builder2.Append(this.ReadCurrentCharacter());
                                        continue;
                                    }
                                    this.Skip();
                                    continue;
                                }
                            }
                            while ((this.flowLevel != 0) || (this.cursor.LineOffset >= num));
                        }
                        break;
                    }
                }
                break;
            }
            if (flag)
            {
                this.simpleKeyAllowed = true;
            }
            return new Scalar(builder.ToString(), ScalarStyle.Plain, start, end);
        }

        private Token ScanTag()
        {
            string str;
            string str2;
            Mark start = this.cursor.Mark();
            if (this.analyzer.Check('<', 1))
            {
                str = string.Empty;
                this.Skip();
                this.Skip();
                str2 = this.ScanTagUri(null, start);
                if (!this.analyzer.Check('>', 0))
                {
                    throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a tag, did not find the expected '>'.");
                }
                this.Skip();
            }
            else
            {
                string head = this.ScanTagHandle(false, start);
                if ((head.Length > 1) && ((head[0] == '!') && (head[head.Length - 1] == '!')))
                {
                    str = head;
                    str2 = this.ScanTagUri(null, start);
                }
                else
                {
                    str2 = this.ScanTagUri(head, start);
                    str = "!";
                    if (str2.Length == 0)
                    {
                        str2 = str;
                        str = string.Empty;
                    }
                }
            }
            if (!this.analyzer.IsWhiteBreakOrZero(0))
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a tag, did not find expected whitespace or line break.");
            }
            return new Tag(str, str2, start, this.cursor.Mark());
        }

        private Token ScanTagDirectiveValue(Mark start)
        {
            this.SkipWhitespaces();
            string handle = this.ScanTagHandle(true, start);
            if (!this.analyzer.IsWhite(0))
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a %TAG directive, did not find expected whitespace.");
            }
            this.SkipWhitespaces();
            string prefix = this.ScanTagUri(null, start);
            if (!this.analyzer.IsWhiteBreakOrZero(0))
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a %TAG directive, did not find expected whitespace or line break.");
            }
            return new TagDirective(handle, prefix, start, start);
        }

        private string ScanTagHandle(bool isDirective, Mark start)
        {
            if (!this.analyzer.Check('!', 0))
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a tag, did not find expected '!'.");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(this.ReadCurrentCharacter());
            while (this.analyzer.IsAlphaNumericDashOrUnderscore(0))
            {
                builder.Append(this.ReadCurrentCharacter());
            }
            if (this.analyzer.Check('!', 0))
            {
                builder.Append(this.ReadCurrentCharacter());
            }
            else if (isDirective && ((builder.Length != 1) || (builder[0] != '!')))
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While parsing a tag directive, did not find expected '!'.");
            }
            return builder.ToString();
        }

        private string ScanTagUri(string head, Mark start)
        {
            StringBuilder builder = new StringBuilder();
            if ((head != null) && (head.Length > 1))
            {
                builder.Append(head.Substring(1));
            }
            while (this.analyzer.IsAlphaNumericDashOrUnderscore(0) || this.analyzer.Check(";/?:@&=+$,.!~*'()[]%", 0))
            {
                if (this.analyzer.Check('%', 0))
                {
                    builder.Append(this.ScanUriEscapes(start));
                    continue;
                }
                builder.Append(this.ReadCurrentCharacter());
            }
            if (builder.Length == 0)
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While parsing a tag, did not find expected tag URI.");
            }
            return builder.ToString();
        }

        private void ScanToNextToken()
        {
            while (true)
            {
                if (this.CheckWhiteSpace())
                {
                    this.Skip();
                    continue;
                }
                this.ProcessComment();
                if (!this.analyzer.IsBreak(0))
                {
                    return;
                }
                this.SkipLine();
                if (this.flowLevel == 0)
                {
                    this.simpleKeyAllowed = true;
                }
            }
        }

        private char ScanUriEscapes(Mark start)
        {
            List<byte> list = new List<byte>();
            int num = 0;
            while (this.analyzer.Check('%', 0) && (this.analyzer.IsHex(1) && this.analyzer.IsHex(2)))
            {
                int num2 = (this.analyzer.AsHex(1) << 4) + this.analyzer.AsHex(2);
                if (num != 0)
                {
                    if ((num2 & 0xc0) != 0x80)
                    {
                        throw new SyntaxErrorException(start, this.cursor.Mark(), "While parsing a tag, find an incorrect trailing UTF-8 octet.");
                    }
                }
                else
                {
                    num = ((num2 & 0x80) != 0) ? (((num2 & 0xe0) != 0xc0) ? (((num2 & 240) != 0xe0) ? (((num2 & 0xf8) != 240) ? 0 : 4) : 3) : 2) : 1;
                    if (num == 0)
                    {
                        throw new SyntaxErrorException(start, this.cursor.Mark(), "While parsing a tag, find an incorrect leading UTF-8 octet.");
                    }
                }
                list.Add((byte) num2);
                this.Skip();
                this.Skip();
                this.Skip();
                if (--num <= 0)
                {
                    char[] chars = Encoding.UTF8.GetChars(list.ToArray());
                    if (chars.Length != 1)
                    {
                        throw new SyntaxErrorException(start, this.cursor.Mark(), "While parsing a tag, find an incorrect UTF-8 sequence.");
                    }
                    return chars[0];
                }
            }
            throw new SyntaxErrorException(start, this.cursor.Mark(), "While parsing a tag, did not find URI escaped octet.");
        }

        private int ScanVersionDirectiveNumber(Mark start)
        {
            int num = 0;
            int num2 = 0;
            while (this.analyzer.IsDigit(0))
            {
                if (++num2 > 9)
                {
                    throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a %YAML directive, find extremely long version number.");
                }
                num = (num * 10) + this.analyzer.AsDigit(0);
                this.Skip();
            }
            if (num2 == 0)
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a %YAML directive, did not find expected version number.");
            }
            return num;
        }

        private Token ScanVersionDirectiveValue(Mark start)
        {
            this.SkipWhitespaces();
            int major = this.ScanVersionDirectiveNumber(start);
            if (!this.analyzer.Check('.', 0))
            {
                throw new SyntaxErrorException(start, this.cursor.Mark(), "While scanning a %YAML directive, did not find expected digit or '.' character.");
            }
            this.Skip();
            return new VersionDirective(new Version(major, this.ScanVersionDirectiveNumber(start)), start, start);
        }

        private void Skip()
        {
            this.cursor.Skip();
            this.analyzer.Buffer.Skip(1);
        }

        private void SkipLine()
        {
            if (this.analyzer.IsCrLf(0))
            {
                this.cursor.SkipLineByOffset(2);
                this.analyzer.Buffer.Skip(2);
            }
            else if (this.analyzer.IsBreak(0))
            {
                this.cursor.SkipLineByOffset(1);
                this.analyzer.Buffer.Skip(1);
            }
            else if (!this.analyzer.IsZero(0))
            {
                throw new InvalidOperationException("Not at a break.");
            }
        }

        private void SkipWhitespaces()
        {
            while (this.analyzer.IsWhite(0))
            {
                this.Skip();
            }
        }

        private void StaleSimpleKeys()
        {
            foreach (SimpleKey key in this.simpleKeys)
            {
                if (key.IsPossible && ((key.Line < this.cursor.Line) || ((key.Index + 0x400) < this.cursor.Index)))
                {
                    if (key.IsRequired)
                    {
                        Mark start = this.cursor.Mark();
                        throw new SyntaxErrorException(start, start, "While scanning a simple key, could not find expected ':'.");
                    }
                    key.IsPossible = false;
                }
            }
        }

        private static bool StartsWith(StringBuilder what, char start) => 
            (what.Length > 0) && (what[0] == start);

        private void UnrollIndent(int column)
        {
            if (this.flowLevel == 0)
            {
                while (this.indent > column)
                {
                    Mark start = this.cursor.Mark();
                    this.tokens.Enqueue(new BlockEnd(start, start));
                    this.indent = this.indents.Pop();
                }
            }
        }

        public bool SkipComments { get; private set; }

        public Token Current { get; private set; }

        public Mark CurrentPosition =>
            this.cursor.Mark();
    }
}

