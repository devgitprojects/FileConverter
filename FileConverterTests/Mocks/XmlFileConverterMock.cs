using CommonFileConverter.Mappers;
using System.IO;
using XmlBinFileConverter.Converters;
using XmlBinFileConverter.Models;

namespace FileConverterTests.ConvertersMocks
{
    internal class XmlFileConverterMock : XmlFileConverter
    {
        public XmlFileConverterMock() : base(new SerializerMock<XmlCarsFile, XmlCar>(), new MappersHolder()) { }

        protected override Stream GetReadStream(string filePath)
        {
            Path.GetFullPath(filePath);
            return new MemoryStream();
        }
        protected override Stream GetWriteStream(string filePath)
        {
            Path.GetFullPath(filePath);
            return new MemoryStream();
        }

    }
}
