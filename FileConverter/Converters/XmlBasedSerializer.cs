using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FileConverter.Converters
{
    internal class XmlBasedSerializer<T> : ISerializer where T : ISerializable
    {
        internal XmlBasedSerializer()
        {
            Serializer = new XmlSerializer(typeof(T));
        }

        private XmlSerializer Serializer { get; set; }

        public object Deserialize(Stream serializationStream)
        {
            return Serializer.Deserialize(serializationStream);
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            Serializer.Serialize(serializationStream, graph);
        }
    }
}
