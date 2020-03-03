using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    public class XmlBasedFileInfo : BaseFileInfo
    {
    }

    [Serializable]
    public class Document : ISerializable
    {
        public Document()
        {
        }

        public Document(List<XmlBasedFileInfo> fileContent)
        {
            Cars = fileContent;
        }

        public Document(SerializationInfo info, StreamingContext context)
        {
            Cars = (List<XmlBasedFileInfo>)info.GetValue("Cars", typeof(List<XmlBasedFileInfo>));
        }

        [XmlElement("Car")]
        public List<XmlBasedFileInfo> Cars { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Cars", this.Cars, typeof(List<XmlBasedFileInfo>));
        }
    }
}
