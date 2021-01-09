namespace WebSocketSharp
{
    using System;

    public class ErrorEventArgs : EventArgs
    {
        private System.Exception _exception;
        private string _message;

        internal ErrorEventArgs(string message) : this(message, null)
        {
        }

        internal ErrorEventArgs(string message, System.Exception exception)
        {
            this._message = message;
            this._exception = exception;
        }

        public System.Exception Exception =>
            this._exception;

        public string Message =>
            this._message;
    }
}

