using CommonFileConverter.Models;
using System.IO;

namespace CommonFileConverter.Interfaces
{
    /// <summary>
    /// Represents serialization engine
    /// </summary>
    public interface ISerializer<T> where T : BaseModel
    {   
        T Deserialize(Stream serializationStream);       
        void Serialize(Stream serializationStream, T graph);
    }
}
