using CommonFileConverter.Converters;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using CommonFileConverter.Serializers;
using XmlBinFileConverter.Models;

namespace XmlBinFileConverter.Converters
{
    public class XmlFileConverter : BaseConverter<XmlBasedFileStructure>
    {
        public XmlFileConverter() : base(new XmlBasedSerializer<XmlBasedFileStructure>(), new MappersHolder())
        {
            Mappers.AddOrUpdate<Mapper<XmlBasedFileStructure, BinaryBasedFileStructure>, XmlBasedFileStructure, BinaryBasedFileStructure>(
                new Mapper<XmlBasedFileStructure, BinaryBasedFileStructure>(".cbin"));
        }

        public XmlFileConverter(ISerializer<XmlBasedFileStructure> serializer, MappersHolder mappersHolder) : base(serializer, mappersHolder) { }
    }
}
