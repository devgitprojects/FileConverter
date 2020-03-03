using System;

namespace FileConverter.Exceptions
{
    /// <summary>
    /// Represents exception occured during file removing
    /// </summary>
    public class RemoveFileException : FileException
    {
        public RemoveFileException() : base() { }
        public RemoveFileException(string message) : base(message) { }
        public RemoveFileException(string message, Exception inner) : base(message, inner) { }
    }
}
