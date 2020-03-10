using FileConverter.Constants;
using FileConverter.Exceptions;
using FileConverter.Extensions;
using FileConverter.Models;
using Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileConverter.Converters
{
    internal class BaseConverter<T> where T : BaseFileStructure
    {
        internal BaseConverter(ISerializer<T> serializer)
        {
            this.Serializer = serializer;
        }

        public ISerializer<T> Serializer { get; protected set; }

        public virtual IDictionary<string, C> Convert<C>(IDictionary<string, T> filesToConvert, ISerializer<C> serializerC, ISerializer<T> serializerT) where C : BaseFileStructure
        {
            //Stream stream = new MemoryStream();
            //using (stream)
            //{
            //    Serializer.Serialize(stream, source);
            //    stream.Seek(0, SeekOrigin.Begin);
            //    //return (T)formatter.Deserialize(stream);
            //}
            ConcurrentDictionary<string, C> converted = new ConcurrentDictionary<string, C>();

            Parallel.ForEach(filesToConvert, file =>
            {
                Stream stream = null;
                try
                {
                    stream = new MemoryStream();
                    serializerT.Serialize(stream, file.Value);
                    stream.Seek(0, SeekOrigin.Begin);
                    var convertedFile = serializerC.Deserialize(stream);
                    converted.AddOrUpdate(file.Key, convertedFile, (p, f) => convertedFile);
                    Logger.Instance.Info("[CONVERT] {0}", file.Key);
                }
                catch (Exception ex)
                {
                    //Logger.Instance.Error(LogMessages.ReadFileExceptionText + " /n {1}", path, ex.ToString());
                    //exceptions.Enqueue(new ReadFileException(String.Format(LogMessages.ReadFileExceptionText, path), ex));
                }
                finally
                {
                    stream.DisposeIfNotNull();
                }
            });

            return null;
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
