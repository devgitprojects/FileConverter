using CommonFileConverter.Constants;
using CommonFileConverter.Exceptions;
using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using CommonFileConverter.Models;
using Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CommonFileConverter.Converters
{
    public class BaseConverter<T> where T : BaseModel
    {
        public BaseConverter(ISerializer<T> serializer, MappersHolder mappersHolder)
        {
            serializer.ThrowArgumentNullExceptionIfNull();
            mappersHolder.ThrowArgumentNullExceptionIfNull();
            Serializer = serializer;
            Mappers = mappersHolder;
        }

        public ISerializer<T> Serializer { get; protected set; }
        public MappersHolder Mappers { get; protected set; }

        public virtual IDictionary<string, TConvertTo> Convert<TConvertTo>(IDictionary<string, T> filesToConvert)
            where TConvertTo : BaseModel, IInitializable<T>, new()
        {
            ConcurrentDictionary<string, TConvertTo> converted = new ConcurrentDictionary<string, TConvertTo>();
            Mapper<T, TConvertTo> mapper = Mappers.GetOrAdd<Mapper<T, TConvertTo>, T, TConvertTo>();
            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(filesToConvert, file =>
            {
                string oldExtension = Path.GetExtension(file.Key);
                string newPath = file.Key.Replace(oldExtension, mapper.ConvertedFileExtension);

                try
                {
                    var convertedFile = mapper.Convert(file.Value);
                    converted.AddOrUpdate(newPath, convertedFile, (p, f) => convertedFile);
                    Logger.Instance.Info("[CONVERT] {0}", file.Key);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(LogMessages.ConvertFileException + " /n {3}", newPath, oldExtension, mapper.ConvertedFileExtension, ex.ToString());
                    exceptions.Enqueue(new ConvertFileException(String.Format(LogMessages.ConvertFileException, newPath, oldExtension, mapper.ConvertedFileExtension), ex));
                }
            });

            exceptions.ThrowAggregateExceptionIfInnerExceptionPresent();

            return converted;
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
                Stream stream = null;
                try
                {
                    stream = GetReadStream(path);
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
                Stream stream = null;
                try
                {
                    stream = GetWriteStream(file.Key);
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

        protected virtual Stream GetReadStream(string filePath)
        {
            return File.OpenRead(filePath);
        }

        protected virtual Stream GetWriteStream(string filePath)
        {
            return File.Open(filePath, FileMode.Create);
        }
    }
}
