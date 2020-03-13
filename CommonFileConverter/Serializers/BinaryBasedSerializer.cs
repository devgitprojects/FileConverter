using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Models;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CommonFileConverter.Serializers
{
    public class BinaryBasedSerializer<T> : ISerializer<T> where T : BaseModel
    {
        public BinaryBasedSerializer() { }

        public virtual T Deserialize(Stream serializationStream)
        {
            serializationStream.ThrowArgumentNullExceptionIfNull();
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            serializationStream.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
            return RawDeserialize(typeof(T), ref buffer) as T;
        }

        public virtual void Serialize(Stream serializationStream, T graph)
        {
            serializationStream.ThrowArgumentNullExceptionIfNull();
            graph.ThrowArgumentNullExceptionIfNull();
            graph.Validate();

            int rawsize = Marshal.SizeOf(graph);
            byte[] rawdata = new byte[rawsize];
            RawSerialize(graph, ref rawdata);
            serializationStream.Write(rawdata, 0, rawdata.Length);
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
    }
}
