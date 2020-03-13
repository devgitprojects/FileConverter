using CommonFileConverter.Converters;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using CommonFileConverter.Serializers;
using XmlBinFileConverter.Models;

namespace XmlBinFileConverter.Converters
{
    public class XmlFileConverter : BaseConverter<XmlCarsFile>
    {
        public XmlFileConverter() : this(new XmlBasedSerializer<XmlCarsFile>(), new MappersHolder()) { }
        public XmlFileConverter(ISerializer<XmlCarsFile> serializer, MappersHolder mappersHolder) : base(serializer, mappersHolder) 
        {
            Mappers.AddOrUpdate<Mapper<XmlCarsFile, BinaryCarsFile>, XmlCarsFile, BinaryCarsFile>(
                new Mapper<XmlCarsFile, BinaryCarsFile>(".cbin"));
        }
    }
}
