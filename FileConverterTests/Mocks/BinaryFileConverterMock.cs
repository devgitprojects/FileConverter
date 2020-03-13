using CommonFileConverter.Mappers;
using System.IO;
using XmlBinFileConverter.Converters;
using XmlBinFileConverter.Models;

namespace FileConverterTests.ConvertersMocks
{
    internal class BinaryFileConverterMock : BinaryFileConverter
    {
        public BinaryFileConverterMock() : base(new SerializerMock<BinaryCarsFile, BinaryCar>(), new MappersHolder()) { }

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
