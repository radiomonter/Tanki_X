namespace Platform.Library.ClientResources.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    public class DiskCacheWriterThread
    {
        private bool running;
        private Queue<DiskCacheWriterRequest> tasks = new Queue<DiskCacheWriterRequest>();

        public void Run()
        {
            this.running = true;
            while (this.running)
            {
                while (true)
                {
                    DiskCacheWriterRequest task = null;
                    object tasks = this.tasks;
                    lock (tasks)
                    {
                        if (this.tasks.Count > 0)
                        {
                            task = this.tasks.Dequeue();
                        }
                    }
                    if (task != null)
                    {
                        this.Write(task);
                    }
                    if (task == null)
                    {
                        Thread.Sleep(50);
                        break;
                    }
                }
            }
        }

        public void Stop()
        {
            this.running = false;
        }

        private void Write(DiskCacheWriterRequest task)
        {
            int num = 3;
            bool flag = false;
            FileStream stream = null;
            while (true)
            {
                try
                {
                    while (true)
                    {
                        if (!File.Exists(task.Path))
                        {
                            try
                            {
                                stream = File.Open(task.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                                stream.Write(task.Data, 0, task.Data.Length);
                                flag = true;
                            }
                            catch (IOException exception)
                            {
                                if (num-- <= 0)
                                {
                                    throw exception;
                                }
                                Thread.Sleep(100);
                            }
                            finally
                            {
                                if (stream != null)
                                {
                                    stream.Close();
                                    stream = null;
                                }
                            }
                            if (!flag)
                            {
                                break;
                            }
                        }
                        return;
                    }
                }
                catch (Exception exception2)
                {
                    task.Error = exception2.Message;
                    return;
                }
                finally
                {
                    task.IsDone = true;
                }
            }
        }

        public DiskCacheWriterRequest Write(string path, byte[] data)
        {
            DiskCacheWriterRequest item = new DiskCacheWriterRequest {
                Data = data,
                Path = path
            };
            lock (this.tasks)
            {
                this.tasks.Enqueue(item);
            }
            return item;
        }
    }
}

