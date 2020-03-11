using CommonFileConverter.Converters;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using CommonFileConverter.Serializers;
using XmlBinFileConverter.Models;

namespace XmlBinFileConverter.Converters
{
    class BinaryFileConverter : BaseConverter<BinaryBasedFileStructure>
    {
        public BinaryFileConverter() : base(new BinaryBasedSerializer<BinaryBasedFileStructure>(), new MappersHolder())
        {
            Mappers.AddOrUpdate<Mapper<BinaryBasedFileStructure, XmlBasedFileStructure>, BinaryBasedFileStructure, XmlBasedFileStructure>(
                new Mapper<BinaryBasedFileStructure, XmlBasedFileStructure>(".cbin"));
        }

        public BinaryFileConverter(ISerializer<BinaryBasedFileStructure> serializer, MappersHolder mappersHolder) : base(serializer, mappersHolder) { }
    }
}
