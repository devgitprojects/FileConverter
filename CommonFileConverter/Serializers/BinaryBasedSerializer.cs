using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Models;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CommonFileConverter.Serializers
{
    public class BinaryBasedSerializer<T> : ISerializer<T> where T : BaseFileStructure
    {
        public virtual T Deserialize(Stream serializationStream)
        {
            serializationStream.ThrowArgumentNullExceptionIfNull();
            T deserialized = default(T);
            using (BinaryReader reader = new BinaryReader(serializationStream))
            {
                byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
                reader.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
                deserialized = RawDeserialize(typeof(T), ref buffer) as T;
            }

            return deserialized;
        }

        public virtual void Serialize(Stream serializationStream, T graph)
        {
            serializationStream.ThrowArgumentNullExceptionIfNull();
            graph.ThrowArgumentNullExceptionIfNull();
            graph.Validate();

            int rawsize = Marshal.SizeOf(graph);
            byte[] rawdata = new byte[rawsize];
            RawSerialize(graph, ref rawdata);
            Write(serializationStream, rawdata);
        }

        protected void RawSerialize(object serializable, ref byte[] rawdataHolder)
        {
            using (GCSafeHandle handle = new GCSafeHandle(rawdataHolder))
            {
                handle.RawSerialize(serializable);
            }
        }

        protected object RawDeserialize(Type objectType, ref byte[] buffer)
        {
            using (GCSafeHandle handle = new GCSafeHandle(buffer))
            {
                return handle.RawDeserialize(objectType);
            }
        }

        protected void Write(Stream serializationStream, byte[] rawdata)
        {
            BinaryWriter writer = null;
            try
            {
                writer = new BinaryWriter(serializationStream);
                writer.Write(rawdata);
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }
    }
}
