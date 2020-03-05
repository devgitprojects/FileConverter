using FileConverter.Models;
using System.IO;
using System.Runtime.Serialization;

namespace FileConverter.Converters
{
    /// <summary>
    /// Represents serialization engine
    /// </summary>
    public interface ISerializer<T> where T : BaseFileStructure
    {   
        T Deserialize(Stream serializationStream);       
        void Serialize(Stream serializationStream, T graph);
    }
}
