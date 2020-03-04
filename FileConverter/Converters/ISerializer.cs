using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace FileConverter.Converters
{
    /// <summary>
    /// Represents serialization engine
    /// </summary>
    public interface ISerializer
    {   
        object Deserialize(Stream serializationStream);       
        void Serialize(Stream serializationStream, object graph);
    }
}
