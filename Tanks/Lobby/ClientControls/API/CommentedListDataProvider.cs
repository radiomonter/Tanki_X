namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class CommentedListDataProvider : DefaultListDataProvider
    {
        private List<string> Comments = new List<string>();

        public override void AddItem(object data)
        {
            this.Comments.Add(string.Empty);
            base.AddItem(data);
        }

        public void AddItem(object data, string comment)
        {
            this.Comments.Add(comment);
            if (!string.IsNullOrEmpty(comment))
            {
                this.CommentCount++;
            }
            this.AddItem(data);
        }

        public override void ClearItems()
        {
            this.Comments.Clear();
            this.CommentCount = 0;
            base.ClearItems();
        }

        public string GetComment(object data)
        {
            int index = base.dataStorage.IndexOf(data);
            return this.Comments[index];
        }

        public bool HasComment(object data)
        {
            int index = base.dataStorage.IndexOf(data);
            return !string.IsNullOrEmpty(this.Comments[index]);
        }

        public override void RemoveItem(object data)
        {
            int index = base.dataStorage.IndexOf(data);
            if (!string.IsNullOrEmpty(this.Comments[index]))
            {
                this.CommentCount--;
            }
            this.Comments.RemoveAt(index);
            base.RemoveItem(data);
        }

        public int CommentCount { get; private set; }
    }
}

