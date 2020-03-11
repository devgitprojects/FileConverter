using CommonFileConverter.Mappers;
using XmlBinFileConverter.Models;

namespace XmlBinFileConverter.Mappers
{
    public class XmlMapper : Mapper<XmlBasedFileStructure, BinaryBasedFileStructure>
    {
        public XmlMapper() : base(".cxml") { }
    }
}
