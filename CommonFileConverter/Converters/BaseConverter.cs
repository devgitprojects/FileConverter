﻿using CommonFileConverter.Constants;
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
    public class BaseConverter<T> where T : BaseFileStructure
    {
        protected BaseConverter(ISerializer<T> serializer)
        {
            serializer.ThrowArgumentNullExceptionIfNull();
            Serializer = serializer;
        }

        public BaseConverter(ISerializer<T> serializer, MappersHolder mappersHolder)
        {
            mappersHolder.ThrowArgumentNullExceptionIfNull();
            Mappers = mappersHolder;
        }

        public ISerializer<T> Serializer { get; protected set; }
        public MappersHolder Mappers { get; protected set; }

        public virtual IDictionary<string, TConvertTo> Convert<TConvertTo>(IDictionary<string, T> filesToConvert)
            where TConvertTo : BaseFileStructure, IInitializable<T>, new()
        {
            ConcurrentDictionary<string, TConvertTo> converted = new ConcurrentDictionary<string, TConvertTo>();
            Mapper<T, TConvertTo> mapper = Mappers.GetOrAdd<Mapper<T, TConvertTo>, T, TConvertTo>();
            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(filesToConvert, file =>
            {
                string convertedFilePath = String.Empty;
                string oldExtension = Path.GetExtension(file.Key);
                string newExtension = file.Key.Replace(oldExtension, mapper.ConvertedFileExtension);

                try
                {
                    var convertedFile = mapper.Convert(file.Value);
                    convertedFilePath = file.Key.Replace(Path.GetExtension(file.Key), mapper.ConvertedFileExtension);
                    converted.AddOrUpdate(convertedFilePath, convertedFile, (p, f) => convertedFile);
                    Logger.Instance.Info("[CONVERT] {0}", file.Key);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(LogMessages.ConvertFileException + " /n {3}", convertedFilePath, oldExtension, mapper.ConvertedFileExtension, ex.ToString());
                    exceptions.Enqueue(new ConvertFileException(String.Format(LogMessages.ConvertFileException, convertedFilePath, oldExtension, mapper.ConvertedFileExtension), ex));
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
