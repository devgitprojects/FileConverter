using System;

namespace CommonFileConverter.Exceptions
{
    /// <summary>
    /// Represents exception occurred during file saving
    /// </summary>
    [Serializable]
    public class ModifyFileException : FileException
    {
        public ModifyFileException() : base() { }
        public ModifyFileException(string message) : base(message) { }
        public ModifyFileException(string message, Exception inner) : base(message, inner) { }
    }
}
