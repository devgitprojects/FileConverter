using FileConverter.Constants;
using FileConverter.Exceptions;
using FileConverter.Models;
using Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileConverter.Converters
{
    internal class Converter : BaseConverter<ISerializable>
    {
        internal Converter(ISerializer serializer)
        {
            this.Serializer = serializer;
        }

        protected override void Convert()
        {
            throw new NotImplementedException();
        }

        public override void Create(IDictionary<string, ISerializable> files)
        {
            if (files == null)
            {
                throw new ArgumentNullException();
            }

            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(files, file =>
            {
                FileStream stream = null;
                try
                {
                    stream = File.Create(file.Key);
                    this.Serializer.Serialize(stream, file.Value);
                    Logger.Instance.Info("[CREATED] {0}", file.Key);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(LogMessages.CreateFileExceptionText + " /n {1}", file.Key, ex.ToString());
                    exceptions.Enqueue(new CreateFileException(String.Format(LogMessages.CreateFileExceptionText, file.Key), ex));
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                    }
                }
            });

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        public override IDictionary<string, ISerializable> Read(params string[] filesPaths)
        {
            if (filesPaths == null)
            {
                throw new ArgumentNullException();
            }

            ConcurrentDictionary<string, ISerializable> files = new ConcurrentDictionary<string, ISerializable>();
            var exceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(filesPaths, path =>
            {
                FileStream stream = null;
                try
                {
                    stream = File.OpenRead(path);
                    ISerializable file = this.Serializer.Deserialize(stream) as ISerializable;
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
                    if (stream != null)
                    {
                        stream.Dispose();
                    }
                }
            });

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }

            return files;
        }

        public override void Save(IDictionary<string, ISerializable> filesToSave)
        {
            if (filesToSave == null)
            {
                throw new ArgumentNullException();
            }

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
                    if (stream != null)
                    {
                        stream.Dispose();
                    }
                }
            });

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
