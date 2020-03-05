using FileConverter.Extensions;
using FileConverter.Models;
using System.IO;
using System.Xml.Serialization;

namespace FileConverter.Converters
{
    internal class XmlBasedSerializer<T> : ISerializer<T> where T : BaseFileStructure
    {
        internal XmlBasedSerializer()
        {
            Serializer = new XmlSerializer(typeof(T));
        }

        private XmlSerializer Serializer { get; set; }

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
