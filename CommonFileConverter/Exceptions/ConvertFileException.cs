using System;

namespace CommonFileConverter.Exceptions
{
    /// <summary>
    /// Represents exception occurred during file conversion
    /// </summary>
    public class ConvertFileException : FileException
    {
        public ConvertFileException() : base() { }
        public ConvertFileException(string message) : base(message) { }
        public ConvertFileException(string message, Exception inner) : base(message, inner) { }
    }
}
