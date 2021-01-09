namespace Platform.Library.ClientResources.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class FileUtils
    {
        public static void DeleteDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                foreach (string str in Directory.GetDirectories(directory))
                {
                    DeleteDirectory(str);
                }
                foreach (string str2 in Directory.GetFiles(directory))
                {
                    File.SetAttributes(str2, FileAttributes.Archive);
                    File.Delete(str2);
                }
                File.SetAttributes(directory, FileAttributes.Archive);
                Directory.Delete(directory);
            }
        }

        [DebuggerHidden]
        public static IEnumerable<string> GetFiles(string fromPath, Func<string, bool> filter = null) => 
            new <GetFiles>c__Iterator0 { 
                fromPath = fromPath,
                filter = filter,
                $PC = -2
            };

        public static void ReplaceLineInFile(string fileName, string contains, string target)
        {
            string[] source = File.ReadAllLines(fileName);
            for (int i = 0; i < source.Count<string>(); i++)
            {
                if (source[i].Contains(contains))
                {
                    source[i] = target;
                }
            }
            File.WriteAllLines(fileName, source);
        }

        [CompilerGenerated]
        private sealed class <GetFiles>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
        {
            internal string fromPath;
            internal string[] $locvar0;
            internal int $locvar1;
            internal string <directory>__1;
            internal Func<string, bool> filter;
            internal IEnumerator<string> $locvar2;
            internal string <file>__2;
            internal string[] $locvar3;
            internal int $locvar4;
            internal string <file>__3;
            internal string $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            if (this.$locvar2 != null)
                            {
                                this.$locvar2.Dispose();
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$locvar0 = Directory.GetDirectories(this.fromPath + "/");
                        this.$locvar1 = 0;
                        break;

                    case 1:
                        goto TR_001B;

                    case 2:
                        goto TR_0002;

                    default:
                        goto TR_0000;
                }
                goto TR_001E;
            TR_0000:
                return false;
            TR_0002:
                this.$locvar4++;
                goto TR_000A;
            TR_0003:
                return true;
            TR_000A:
                while (true)
                {
                    if (this.$locvar4 < this.$locvar3.Length)
                    {
                        this.<file>__3 = this.$locvar3[this.$locvar4];
                        if ((this.filter == null) || this.filter(this.<file>__3))
                        {
                            this.$current = this.<file>__3;
                            if (!this.$disposing)
                            {
                                this.$PC = 2;
                            }
                            break;
                        }
                    }
                    else
                    {
                        this.$PC = -1;
                        goto TR_0000;
                    }
                    goto TR_0002;
                }
                goto TR_0003;
            TR_001B:
                try
                {
                    switch (num)
                    {
                        default:
                            while (true)
                            {
                                if (this.$locvar2.MoveNext())
                                {
                                    this.<file>__2 = this.$locvar2.Current;
                                    if ((this.filter != null) && !this.filter(this.<file>__2))
                                    {
                                        continue;
                                    }
                                    this.$current = this.<file>__2;
                                    if (!this.$disposing)
                                    {
                                        this.$PC = 1;
                                    }
                                    flag = true;
                                }
                                else
                                {
                                    this.$locvar1++;
                                    goto TR_001E;
                                }
                                break;
                            }
                            break;
                    }
                }
                finally
                {
                    if (flag)
                    {
                    }
                    if (this.$locvar2 != null)
                    {
                        this.$locvar2.Dispose();
                    }
                }
                goto TR_0003;
            TR_001E:
                while (true)
                {
                    if (this.$locvar1 < this.$locvar0.Length)
                    {
                        this.<directory>__1 = this.$locvar0[this.$locvar1];
                        this.$locvar2 = FileUtils.GetFiles(this.<directory>__1, this.filter).GetEnumerator();
                        num = 0xfffffffd;
                    }
                    else
                    {
                        this.$locvar3 = Directory.GetFiles(this.fromPath + "/");
                        this.$locvar4 = 0;
                        goto TR_000A;
                    }
                    break;
                }
                goto TR_001B;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new FileUtils.<GetFiles>c__Iterator0 { 
                    fromPath = this.fromPath,
                    filter = this.filter
                };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();

            string IEnumerator<string>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

