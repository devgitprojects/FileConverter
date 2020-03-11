using System;

namespace CommonFileConverter.Exceptions
{
    /// <summary>
    /// Represents exception occurred during file removing
    /// </summary>
    public class RemoveFileException : FileException
    {
        public RemoveFileException() : base() { }
        public RemoveFileException(string message) : base(message) { }
        public RemoveFileException(string message, Exception inner) : base(message, inner) { }
    }
}
