using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Models;
using System.IO;
using System.Xml.Serialization;

namespace CommonFileConverter.Serializers
{
    public class XmlBasedSerializer<T> : ISerializer<T> where T : BaseFileStructure
    {
        public XmlBasedSerializer()
        {
            Serializer = new XmlSerializer(typeof(T));
        }

        private XmlSerializer Serializer { get; set; }

        public virtual T Deserialize(Stream serializationStream)
        {
            serializationStream.ThrowArgumentNullExceptionIfNull();
            return (T)Serializer.Deserialize(serializationStream);
        }

        public virtual void Serialize(Stream serializationStream, T graph)
        {
            serializationStream.ThrowArgumentNullExceptionIfNull();
            graph.ThrowArgumentNullExceptionIfNull();
            graph.Validate();
            Serializer.Serialize(serializationStream, graph);
        }
    }
}
