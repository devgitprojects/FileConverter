using FileConverter.Constants;
using FileConverter.Exceptions;
using FileConverter.Extensions;
using FileConverter.Models;
using Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileConverter.Converters
{
    internal class BaseConverter<T> where T : BaseFileStructure
    {
        internal BaseConverter(ISerializer<T> serializer)
        {
            this.Serializer = serializer;
        }

        protected ISerializer<T> Serializer { get; set; }

        public virtual void Convert()
        {
            throw new NotImplementedException();
        }

        public void Delete(params string[] filesToDelete)
        {
            filesToDelete.ThrowArgumentNullExceptionIfNull();

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

            exceptions.ThrowAggregateExceptionIfInnerExceptionPresent();
        }

        public virtual IDictionary<string, T> Read(params string[] filesPaths)
        {            
            filesPaths.ThrowArgumentNullExceptionIfNull();           

            ConcurrentDictionary<string, T> files = new ConcurrentDictionary<string, T>();
            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(filesPaths, path =>
            {
                FileStream stream = null;
                try
                {
                    stream = File.OpenRead(path);
                    T file = this.Serializer.Deserialize(stream);
                    files.AddOrUpdate(path, file, (p, f) => file);
                    Logger.Instance.Info("[READ] {0}", path);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(LogMessages.ReadFileExceptionText + " /n {1}", path, ex.ToString());
                    exceptions.Enqueue(new ReadFileException(String.Format(LogMessages.ReadFileExceptionText, path), ex));
                }
                finally
                {
                    stream.DisposeIfNotNull();
                }
            });

            exceptions.ThrowAggregateExceptionIfInnerExceptionPresent();

            return files;            
        }

        public virtual void Save(IDictionary<string, T> filesToSave)
        {            
            filesToSave.ThrowArgumentNullExceptionIfNull();

            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(filesToSave, file =>
            {
                FileStream stream = null;
                try
                {
                    stream = File.Open(file.Key, FileMode.Create);
                    this.Serializer.Serialize(stream, file.Value);
                    Logger.Instance.Info("[SAVE] {0}", file.Key);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(LogMessages.ModifyFileExceptionText + " /n {1}", file.Key, ex.ToString());
                    exceptions.Enqueue(new ModifyFileException(String.Format(LogMessages.ModifyFileExceptionText, file.Key), ex));
                }
                finally
                {
                    stream.DisposeIfNotNull();
                }
            });

            exceptions.ThrowAggregateExceptionIfInnerExceptionPresent();
        }
    }
}
