using System;

namespace CommonFileConverter.Exceptions
{
    /// <summary>
    /// Represents exception occurred during file reading
    /// </summary>
    [Serializable]
    public class ReadFileException : FileException
    {
        public ReadFileException() : base() { }
        public ReadFileException(string message) : base(message) { }
        public ReadFileException(string message, Exception inner) : base(message, inner) { }
    }
}
