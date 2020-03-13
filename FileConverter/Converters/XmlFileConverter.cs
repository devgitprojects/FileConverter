using CommonFileConverter.Converters;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using CommonFileConverter.Serializers;
using XmlBinFileConverter.Models;

namespace XmlBinFileConverter.Converters
{
    public class XmlFileConverter : BaseConverter<XmlCarsFile>
    {
        public XmlFileConverter() : base(new XmlBasedSerializer<XmlCarsFile>(), new MappersHolder())
        {
            Mappers.AddOrUpdate<Mapper<XmlCarsFile, BinaryCarsFile>, XmlCarsFile, BinaryCarsFile>(
                new Mapper<XmlCarsFile, BinaryCarsFile>(".cbin"));
        }

        public XmlFileConverter(ISerializer<XmlCarsFile> serializer, MappersHolder mappersHolder) : base(serializer, mappersHolder) { }
    }
}
