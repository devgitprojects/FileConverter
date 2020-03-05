using FileConverter.Extensions;
using FileConverter.Models;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileConverter.Converters
{
    internal class BinaryBasedSerializer<T> : ISerializer<T> where T : BaseFileStructure
    {
        internal BinaryBasedSerializer()
        {
            Serializer = new BinaryFormatter();
        }

        private BinaryFormatter Serializer { get; set; }

        public T Deserialize(Stream serializationStream)
        {
            serializationStream.ThrowArgumentNullExceptionIfNull();
            return (T)Serializer.Deserialize(serializationStream);
        }

        public void Serialize(Stream serializationStream, T graph)
        {
            serializationStream.ThrowArgumentNullExceptionIfNull();
            graph.ThrowArgumentNullExceptionIfNull();
            Serializer.Serialize(serializationStream, graph);
        }
    }
}
