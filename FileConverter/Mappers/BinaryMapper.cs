using CommonFileConverter.Mappers;
using XmlBinFileConverter.Models;

namespace XmlBinFileConverter.Mappers
{
    public class BinaryMapper : Mapper<BinaryBasedFileStructure, XmlBasedFileStructure>
    {
        public BinaryMapper() : base(".cbin") { }
    }
}
