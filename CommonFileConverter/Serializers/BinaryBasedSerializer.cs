using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Models;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CommonFileConverter.Serializers
{
    public class BinaryBasedSerializer<T> : ISerializer<T> where T : BaseFileStructure
    {
        public BinaryBasedSerializer()
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
            graph.Validate();
            Serializer.Serialize(serializationStream, graph);
        }
    }
}
