using FileConverter.Constants;
using FileConverter.Exceptions;
using Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FileConverter.Converters
{
    abstract class BaseConverter<T> where T : ISerializable
    {
        protected ISerializer Serializer { get; set; }

        protected abstract void Convert();
        public abstract void Create(IDictionary<string, T> files);

        public void Delete(params string[] filesToDelete)
        {
            if (filesToDelete == null)
            {
                throw new ArgumentNullException();
            }

            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(filesToDelete, file =>
            {
                if (File.Exists(file))
                {
                    try
                    {
                        File.Delete(file);
                        Logger.Instance.Info("[DELETE] {0}", file);
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.Error(LogMessages.RemoveFileExceptionText + " /n {1}", file, ex.ToString());
                        exceptions.Enqueue(new RemoveFileException(String.Format(LogMessages.RemoveFileExceptionText, file), ex));
                    }
                }
            });

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        public abstract IDictionary<string, T> Read(params string[] filesPaths);
        public abstract void Save(IDictionary<string, T> filesToSave);
    }
}
