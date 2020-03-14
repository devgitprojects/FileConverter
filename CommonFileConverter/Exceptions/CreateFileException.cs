using System;

namespace CommonFileConverter.Exceptions
{
    /// <summary>
    /// Represents exception occurred during file creating
    /// </summary>
    [Serializable]
    public class CreateFileException : FileException
    {
        public CreateFileException() : base() { }
        public CreateFileException(string message) : base(message) { }
        public CreateFileException(string message, Exception inner) : base(message, inner) { }
    }
}
