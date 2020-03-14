using System;

namespace CommonFileConverter.Exceptions
{
    /// <summary>
    /// Represents base class for all exceptions occurred during file handling
    /// </summary>
    [Serializable]
    public class FileException : Exception
    {
        public FileException() { }
        public FileException(string message) : base(message) { }
        public FileException(string message, Exception inner) : base(message, inner) { }
    }
}
