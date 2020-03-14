using CommonFileConverter.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using XmlBinFileConverter.Models;

namespace FileConverterTests
{
    internal class TestingHelper
    {
        public static BaseModel CreateXmlCarFile()
        {
            return CreateCarFile<XmlCarsFile, XmlCar>();
        }

        public static BaseModel CreateBinaryCarFile()
        {
            return CreateCarFile<BinaryCarsFile, BinaryCar>();
        }

        public static TFile CreateCarFile<TFile, TModel>()
            where TFile : BaseCarsFile<TModel>, new()
            where TModel : XmlCar, new()
        {
            TFile file = new TFile();
            file.Cars = CreateCarsCollection<TModel>();
            return file;
        }

        public static CarsCollection<T> CreateCarsCollection<T>() where T : XmlCar, new()
        {
            CarsCollection<T> cars = new CarsCollection<T>();
            for (uint i = 0; i < TestContext.CurrentContext.Random.NextByte(1, byte.MaxValue); i++)
            {
                var car = CreateCar<T>();
                if (!cars.Contains(car.BrandName))
                {
                    cars.Add(car);
                }
            }

            return cars;
        }

        public static T CreateCar<T>() where T : XmlCar, new()
        {
            byte ALLOWED_LENGHT_OF_BRANDNAME_OF_BINARYCAR = 2;
            return new T
            {
                BrandName = TestContext.CurrentContext.Random.GetString(ALLOWED_LENGHT_OF_BRANDNAME_OF_BINARYCAR, "ABCDEFGHJKLMNOPQRSTUVWXYZ"),
                Date = RandomDate(),
                Price = TestContext.CurrentContext.Random.NextUInt()
            };
        }

        public static IDictionary<string, TFile> CreateFiles<TFile, TModel>(byte maxFilesCount = 0)
            where TFile : BaseCarsFile<TModel>, new()
            where TModel : XmlCar, new()

        {
            byte filesCount = maxFilesCount == 0 ? TestContext.CurrentContext.Random.NextByte(1, byte.MaxValue) : maxFilesCount;
            IDictionary<string, TFile> files = new Dictionary<string, TFile>();
            for (int i = 0; i < filesCount; i++)
            {

                var file = CreateCarFile<TFile, TModel>();
                files.Add(Path.Combine(Path.GetTempPath(), String.Format("{0}.cxml", i)), file);
            }

            return files;
        }

        private static DateTime RandomDate()
        {
            return DateTime.Today.AddDays(TestContext.CurrentContext.Random.NextSByte());
        }
    }
}
