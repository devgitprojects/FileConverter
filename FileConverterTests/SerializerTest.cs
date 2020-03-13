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
    public class SerializerTest<TSerializer, TModel> 
        where TSerializer : ISerializer<TModel>, new()
        where TModel : BaseModel
    {
        public SerializerTest()
        {
            serializer = new TSerializer();

            if (typeof(TModel) == typeof(XmlCar))
            {
                model = CreateCar<XmlCar>();
            }
            if (typeof(TModel) == typeof(BinaryCar))
            {
                model = CreateCar<BinaryCar>();
            }
            else if (typeof(TModel) == typeof(XmlCarsFile))
            {
                model = new XmlCarsFile();
                ((XmlCarsFile)model).Cars = CreateCarsCollection<XmlCar>();
            }
            else if (typeof(TModel) == typeof(BinaryCarsFile))
            {
                model = new BinaryCarsFile();
                ((BinaryCarsFile)model).Cars = CreateCarsCollection<BinaryCar>();
            }
        }

        [TestCase]
        public void SerializeTest()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, (TModel)model);
            }
        }

        [TestCase]
        public void DeserializeTest()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, (TModel)model);
                stream.Position = 0;
                TModel cloned = serializer.Deserialize(stream);
                Assert.AreEqual(cloned, model);                
            }
        }

        private CarsCollection<T> CreateCarsCollection<T>() where T : XmlCar, new()
        {
            CarsCollection<T> cars = new CarsCollection<T>();
            for (uint i = 0; i < TestContext.CurrentContext.Random.NextByte(); i++)
            {
                var car = CreateCar<T>();
                if (!cars.Contains(car.BrandName))
                {
                    cars.Add(car);
                }
            }

            return cars;
        }

        private T CreateCar<T>() where T : XmlCar, new()
        {
            byte ALLOWED_LENGHT_OF_BRANDNAME_OF_BINARYCAR = 2;
            return new T
            {
                BrandName = TestContext.CurrentContext.Random.GetString(ALLOWED_LENGHT_OF_BRANDNAME_OF_BINARYCAR, "ABCDEFGHJKLMNOPQRSTUVWXYZ"),
                Date = RandomDate(),
                Price = TestContext.CurrentContext.Random.NextUInt()
            };            
        }

        DateTime RandomDate()
        {
            return DateTime.Today.AddDays(TestContext.CurrentContext.Random.NextSByte());
        }

        private TSerializer serializer;
        private BaseModel model;
    }
}
