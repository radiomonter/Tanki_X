namespace Platform.Library.ClientResources.API
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class FileBackupTransaction : IDisposable
    {
        private Stack<Task> history = new Stack<Task>();

        public void Commit()
        {
            while (this.history.Count > 0)
            {
                try
                {
                    this.history.Pop().Commit();
                }
                catch
                {
                    this.history.Clear();
                    throw;
                }
            }
        }

        public void CopyFile(string fromPath, string toPath)
        {
            CopyTask t = new CopyTask(fromPath, toPath);
            t.Run();
            this.history.Push(t);
        }

        public void DeleteFile(string path)
        {
            DeleteTask t = new DeleteTask(path);
            t.Run();
            this.history.Push(t);
        }

        public void Dispose()
        {
            this.Rollback();
        }

        public void ReplaceFile(string fromPath, string toPath)
        {
            if (File.Exists(toPath))
            {
                this.DeleteFile(toPath);
            }
            else
            {
                string directoryName = Path.GetDirectoryName(toPath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
            }
            this.CopyFile(fromPath, toPath);
        }

        public void Rollback()
        {
            while (this.history.Count > 0)
            {
                this.history.Pop().Rollback();
            }
        }

        private class CopyTask : FileBackupTransaction.Task
        {
            private string formPath;
            private string toPath;

            public CopyTask(string formPath, string toPath)
            {
                this.formPath = formPath;
                this.toPath = toPath;
            }

            public void Commit()
            {
            }

            public void Rollback()
            {
                if (File.Exists(this.toPath))
                {
                    File.SetAttributes(this.toPath, FileAttributes.Archive);
                    File.Delete(this.toPath);
                }
            }

            public FileBackupTransaction.Task Run()
            {
                File.Copy(this.formPath, this.toPath);
                return this;
            }
        }

        private class DeleteTask : FileBackupTransaction.Task
        {
            private string path;
            private string backupPath;

            public DeleteTask(string path)
            {
                this.path = path;
                this.backupPath = $"{path}.bck";
            }

            public void Commit()
            {
                File.SetAttributes(this.backupPath, FileAttributes.Archive);
                File.Delete(this.backupPath);
            }

            public void Rollback()
            {
                if (File.Exists(this.path))
                {
                    File.SetAttributes(this.path, FileAttributes.Archive);
                    File.Delete(this.path);
                }
                File.Copy(this.backupPath, this.path);
                File.SetAttributes(this.backupPath, FileAttributes.Archive);
                File.Delete(this.backupPath);
            }

            public FileBackupTransaction.Task Run()
            {
                File.SetAttributes(this.path, FileAttributes.Archive);
                if (File.Exists(this.backupPath))
                {
                    File.SetAttributes(this.backupPath, FileAttributes.Archive);
                    File.Delete(this.backupPath);
                }
                File.Copy(this.path, this.backupPath);
                File.Delete(this.path);
                return this;
            }
        }

        private interface Task
        {
            void Commit();
            void Rollback();
            FileBackupTransaction.Task Run();
        }
    }
}

