using System;

namespace FileConverter.Exceptions
{
    /// <summary>
    /// Represents exception occured during file creating
    /// </summary>
    public class CreateFileException : FileException
    {
        public CreateFileException() : base() { }
        public CreateFileException(string message) : base(message) { }
        public CreateFileException(string message, Exception inner) : base(message, inner) { }
    }
}
