using System;

namespace FileConverter.Exceptions
{
    /// <summary>
    /// Represents exception occured during file saving
    /// </summary>
    public class SaveFileException : FileException
    {
        public SaveFileException() : base() { }
        public SaveFileException(string message) : base(message) { }
        public SaveFileException(string message, Exception inner) : base(message, inner) { }
    }
}
