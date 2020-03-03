using System;
using System.Collections.Generic;
using System.Linq;

namespace FileConverter.Exceptions
{
    /// <summary>
    /// Represents base class for all exceptions occured during file handling
    /// </summary>
    public class FileException : Exception
    {
        public FileException() { }
        public FileException(string message) : base(message) { }
        public FileException(string message, Exception inner) : base(message, inner) { }
    }
}
