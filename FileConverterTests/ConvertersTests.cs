using CommonFileConverter.Converters;
using CommonFileConverter.Exceptions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using FileConverterTests.ConvertersMocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using XmlBinFileConverter.Models;

namespace FileConverterTests
{
    [TestFixture(typeof(BinaryFileConverterMock), typeof(XmlCarsFile), typeof(XmlCar), typeof(BinaryCarsFile), typeof(BinaryCar))]
    [TestFixture(typeof(XmlFileConverterMock), typeof(BinaryCarsFile), typeof(BinaryCar), typeof(XmlCarsFile), typeof(XmlCar))]
    internal class ConvertersTests<TConverter, TConvertedFile, TConvertedCar, TSourceFile, TSourceCar>
        where TConverter : BaseConverter<TSourceFile>, new()
        where TConvertedFile : BaseCarsFile<TConvertedCar>, IInitializable<TSourceFile>, new()
        where TConvertedCar : XmlCar, new()
        where TSourceFile : BaseCarsFile<TSourceCar>, new()
        where TSourceCar : XmlCar, new()
    {
        public ConvertersTests()
        {
            converter = new TConverter();
            byte filesCount = TestContext.CurrentContext.Random.NextByte(1, byte.MaxValue);
            files = TestingHelper.CreateFiles<TSourceFile, TSourceCar>(filesCount);
        }

        [TestCase]
        public void SaveTestPositive()
        {
            TestContext.Out.WriteLine("SaveTestPositive");

            converter.Save(files);
        }

        [TestCase]
        public void ReadTestPositive()
        {
            TestContext.Out.WriteLine("ReadTestPositive");

            IDictionary<string, TSourceFile> readfiles = converter.Read(files.Keys.ToArray());
            Assert.AreEqual(readfiles.Count, files.Count);
        }

        [TestCase]
        public void ConvertTestPositive()
        {
            TestContext.Out.WriteLine("ConvertTestPositive");

            IDictionary<string, TConvertedFile> convertedFiles = converter.Convert<TConvertedFile>(files);
            converter.Mappers.TryGet<Mapper<TSourceFile, TConvertedFile>, TSourceFile, TConvertedFile>(out Mapper<TSourceFile, TConvertedFile> mapper);

            Assert.AreEqual(convertedFiles.Count, files.Count);

            foreach (var path in convertedFiles.Keys)
            {
                Assert.AreEqual(Path.GetExtension(path), mapper.ConvertedFileExtension, path);
            }

            var convertedAndSource = convertedFiles.OrderBy(x => x.Key).Zip(files.OrderBy(x => x.Key), (c, s) => new { Converted = c, Source = s });

            foreach (var cs in convertedAndSource)
            {
                Assert.AreEqual(Path.GetExtension(cs.Converted.Key), mapper.ConvertedFileExtension, cs.Converted.Key);
                
                Assert.AreEqual(cs.Converted.Value.Cars.Count, cs.Source.Value.Cars.Count, cs.Converted.Key);

                var convertedAndSourceCars = cs.Converted.Value.Cars.Zip(cs.Source.Value.Cars, (c, s) => new { ConvertedCar = c, SourceCar = s });

                foreach (var csCars in convertedAndSourceCars)
                {
                    Assert.AreEqual(csCars.ConvertedCar.BrandName, csCars.SourceCar.BrandName, cs.Converted.Key);
                    Assert.AreEqual(csCars.ConvertedCar.Date, csCars.SourceCar.Date, cs.Converted.Key);
                    Assert.AreEqual(csCars.ConvertedCar.Price, csCars.SourceCar.Price, cs.Converted.Key);
                }
            }
        }

        [TestCase]
        public void SaveTestExceptional()
        {
            TestContext.Out.WriteLine("SaveTestExceptional");

            var files = TestingHelper.CreateFiles<TSourceFile, TSourceCar>(1);
            files.FirstOrDefault().Value.Cars = null;

            var aex = Assert.Throws<AggregateException>(() => converter.Save(files));

            Assert.AreEqual(aex.InnerExceptions.Count, 1);
            var inner = aex.InnerExceptions.FirstOrDefault();
            Assert.AreEqual(inner.GetType(), typeof(ModifyFileException));
            Assert.AreEqual(inner.InnerException.GetType(), typeof(ValidationException));
        }

        [TestCase]
        public void ReadTestExceptional()
        {
            TestContext.Out.WriteLine("ReadTestExceptional");

            var filetoRead = new Dictionary<string, TSourceFile>();
            var file = TestingHelper.CreateCarFile<TSourceFile, TSourceCar>();
            filetoRead.Add("failedPath***!!!$:::;;;;", file);

            var aex = Assert.Throws<AggregateException>(() => converter.Read(filetoRead.Keys.ToArray()));

            Assert.AreEqual(aex.InnerExceptions.Count, 1);
            Assert.AreEqual(aex.InnerExceptions.FirstOrDefault().GetType(), typeof(ReadFileException));
        }

        [TestCase]
        public void ConvertTestExceptional()
        {
            TestContext.Out.WriteLine("ConvertTestExceptional");

            var files = TestingHelper.CreateFiles<TSourceFile, TSourceCar>(1);
            files[files.FirstOrDefault().Key] = null;

            var aex = Assert.Throws<AggregateException>(() => converter.Convert<TConvertedFile>(files));

            Assert.AreEqual(aex.InnerExceptions.Count, 1);
            Assert.AreEqual(aex.InnerExceptions.FirstOrDefault().GetType(), typeof(ConvertFileException));


        }

        private IDictionary<string, TSourceFile> files;
        private TConverter converter;
    }
}
