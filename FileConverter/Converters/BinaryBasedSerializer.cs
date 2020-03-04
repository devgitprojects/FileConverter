using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileConverter.Converters
{
    internal class BinaryBasedSerializer : ISerializer
    {
        internal BinaryBasedSerializer()
        {
            Serializer = new BinaryFormatter();
        }

        private BinaryFormatter Serializer { get; set; }

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
