using System;

namespace FileConverter.Exceptions
{
    /// <summary>
    /// Represents exception occured during file reading
    /// </summary>
    public class ReadFileException : FileException
    {
        public ReadFileException() : base() { }
        public ReadFileException(string message) : base(message) { }
        public ReadFileException(string message, Exception inner) : base(message, inner) { }
    }
}
