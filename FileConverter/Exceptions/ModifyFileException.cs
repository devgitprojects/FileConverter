using System;

namespace FileConverter.Exceptions
{
    /// <summary>
    /// Represents exception occured during file saving
    /// </summary>
    public class ModifyFileException : FileException
    {
        public ModifyFileException() : base() { }
        public ModifyFileException(string message) : base(message) { }
        public ModifyFileException(string message, Exception inner) : base(message, inner) { }
    }
}
