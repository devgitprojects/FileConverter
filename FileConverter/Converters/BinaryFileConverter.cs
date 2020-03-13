using CommonFileConverter.Converters;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using XmlBinFileConverter.Models;
using XmlBinFileConverter.Serializers;

namespace XmlBinFileConverter.Converters
{
    public class BinaryFileConverter : BaseConverter<BinaryCarsFile>
    {
        public BinaryFileConverter() : this(new BinaryCarsSerializer(), new MappersHolder()) { }
        public BinaryFileConverter(ISerializer<BinaryCarsFile> serializer, MappersHolder mappersHolder) : base(serializer, mappersHolder)
        {
            Mappers.AddOrUpdate<Mapper<BinaryCarsFile, XmlCarsFile>, BinaryCarsFile, XmlCarsFile>(
                new Mapper<BinaryCarsFile, XmlCarsFile>(".cxml"));
        }
    }
}
