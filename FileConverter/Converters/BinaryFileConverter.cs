using CommonFileConverter.Converters;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using CommonFileConverter.Serializers;
using XmlBinFileConverter.Models;

namespace XmlBinFileConverter.Converters
{
    public class BinaryFileConverter : BaseConverter<BinaryBasedFileStructure>
    {
        public BinaryFileConverter() : base(new BinaryBasedSerializer<BinaryBasedFileStructure>(), new MappersHolder())
        {
            Mappers.AddOrUpdate<Mapper<BinaryBasedFileStructure, XmlBasedFileStructure>, BinaryBasedFileStructure, XmlBasedFileStructure>(
                new Mapper<BinaryBasedFileStructure, XmlBasedFileStructure>(".cxml"));
        }

        public BinaryFileConverter(ISerializer<BinaryBasedFileStructure> serializer, MappersHolder mappersHolder) : base(serializer, mappersHolder) { }
    }
}
