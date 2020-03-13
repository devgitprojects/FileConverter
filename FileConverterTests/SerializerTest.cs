using CommonFileConverter.Interfaces;
using CommonFileConverter.Models;
using CommonFileConverter.Serializers;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.IO;
using XmlBinFileConverter.Models;
using XmlBinFileConverter.Serializers;

namespace FileConverterTests
{
    [TestFixture(typeof(XmlBasedSerializer<XmlCar>), typeof(XmlCar))]
    [TestFixture(typeof(XmlBasedSerializer<XmlCarsFile>), typeof(XmlCarsFile))]
    [TestFixture(typeof(BinaryBasedSerializer<BinaryCar>), typeof(BinaryCar))]
    [TestFixture(typeof(BinaryCarsSerializer), typeof(BinaryCarsFile))]
    internal class SerializerTest<TSerializer, TModel> 
        where TSerializer : ISerializer<TModel>, new()
        where TModel : BaseModel
    {
        public SerializerTest()
        {
            serializer = new TSerializer();

            if (typeof(TModel) == typeof(XmlCar))
            {
                model = TestingHelper.CreateCar<XmlCar>();
            }
            if (typeof(TModel) == typeof(BinaryCar))
            {
                model = TestingHelper.CreateCar<BinaryCar>();
            }
            else if (typeof(TModel) == typeof(XmlCarsFile))
            {
                model = TestingHelper.CreateCarFile<XmlCarsFile, XmlCar>();
            }
            else if (typeof(TModel) == typeof(BinaryCarsFile))
            {
                model = TestingHelper.CreateCarFile<BinaryCarsFile, BinaryCar>();
            }
        }

        [TestCase]
        public void SerializeTest()
        {
            TestContext.Out.WriteLine("SerializeTest");

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, (TModel)model);
            }
        }

        [TestCase]
        public void DeserializeTest()
        {
            TestContext.Out.WriteLine("DeserializeTest");

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, (TModel)model);
                stream.Position = 0;
                TModel cloned = serializer.Deserialize(stream);
                Assert.AreEqual(cloned, model);                
            }
        }

        private TSerializer serializer;
        private BaseModel model;
    }
}
