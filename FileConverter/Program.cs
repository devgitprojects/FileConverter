using FileConverter.Converters;
using FileConverter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                BaseConverter<XmlBasedFileStructure> xmlConv = new BaseConverter<XmlBasedFileStructure>(new XmlBasedSerializer<XmlBasedFileStructure>());
                BaseConverter<BinaryBasedFileStructure> binConv = new BaseConverter<BinaryBasedFileStructure>(new BinaryBasedSerializer<BinaryBasedFileStructure>());

                string path = @"C:\Temp";

                CarsCollection<XmlCar> cars = new CarsCollection<XmlCar>();
                cars.Add(
                    new XmlCar
                    {
                        Date = DateTime.Now,
                        BrandName = "Nissan",
                        Price = 1,

                    });

                cars.Add(new XmlCar
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandName = "Mazda",
                    Price = 2
                });

                XmlBasedFileStructure file = new XmlBasedFileStructure(cars);
                Dictionary<string, XmlBasedFileStructure> files = new Dictionary<string, XmlBasedFileStructure>();

                for (int i = 0; i < 1; i++)
                {
                    files.Add(Path.Combine(path, String.Format("{0}.cxml", i)), file);
                }

                Mapper<XmlBasedFileStructure, BinaryBasedFileStructure> mapperXmlToBi = new Mapper<XmlBasedFileStructure, BinaryBasedFileStructure>();
                BinaryBasedFileStructure binary = mapperXmlToBi.Convert(file);

                Mapper<BinaryBasedFileStructure, TestBaseFileStructure> mapperBinToSome = new Mapper<BinaryBasedFileStructure, TestBaseFileStructure>();
                Mapper<BinaryBasedFileStructure, XmlBasedFileStructure> mapperBinToXml = new Mapper<BinaryBasedFileStructure, XmlBasedFileStructure>();

                TestBaseFileStructure xml1 = mapperBinToSome.Convert(binary);
                IConvertible<BinaryBasedFileStructure> cb = binary;
                XmlBasedFileStructure xml = cb.Convert(mapperBinToXml);
                TestBaseFileStructure test = cb.Convert(mapperBinToSome);
                //IConvertible<TestBaseFileStructure> cb = binary;
                //cb.Convert(mapperBinToXml);
                xmlConv.Save(files);

                var read = xmlConv.Read(files.Keys.ToArray());

                xmlConv.Convert<BinaryBasedFileStructure>(read, binConv.Serializer, new BinaryBasedSerializer<XmlBasedFileStructure>());

                var fileRead = read.FirstOrDefault();
                var doc1 = fileRead.Value;
                doc1.Cars["Mazda"].BrandName = "SUZUKI";
                doc1.Cars.Remove("Nissan");

                xmlConv.Save(read);

                read = xmlConv.Read(read.Keys.ToArray());

                doc1 = read.FirstOrDefault().Value;
                doc1.Cars.Add(new XmlCar
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandName = "BMW",
                    Price = 22
                });
                doc1.Cars.Add(new XmlCar
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandName = "ZAZ",
                    Price = 333
                });
                doc1.Cars.Add(new XmlCar
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandName = "WRANGLER",
                    Price = 444
                });


                xmlConv.Save(read);
                read = xmlConv.Read(read.Keys.ToArray());


                //string path = @"C:\Temp";

                //CarsCollection<BinaryCar> cars = new CarsCollection<BinaryCar>();
                //cars.Add(
                //    new BinaryCar
                //    {
                //        Date = DateTime.Now,
                //        BrandName = "Nissan",
                //        Price = 1,

                //    });

                //cars.Add(new BinaryCar
                //{
                //    Date = DateTime.Now.AddHours(1),
                //    BrandName = "Mazda",
                //    Price = 2
                //});

                //BinaryBasedFileStructure file = new BinaryBasedFileStructure(cars);
                //Dictionary<string, BinaryBasedFileStructure> files = new Dictionary<string, BinaryBasedFileStructure>();


                //for (int i = 0; i < 1; i++)
                //{
                //    files.Add(Path.Combine(path, String.Format("{0}.cxml", i)), file);
                //}

                //binConv.Save(files);

                //var read = binConv.Read(files.Keys.ToArray());

                //var fileRead = read.FirstOrDefault();
                //var doc1 = fileRead.Value;
                //doc1.Cars["Mazda"].BrandName = "SUZUKI";
                //doc1.Cars.Remove("Nissan");

                //binConv.Save(read);

                //read = binConv.Read(read.Keys.ToArray());

                //doc1 = read.FirstOrDefault().Value;
                //doc1.Cars.Add(new BinaryCar
                //{
                //    Date = DateTime.Now.AddHours(1),
                //    BrandName = "BMW",
                //    Price = 22
                //});
                //doc1.Cars.Add(new BinaryCar
                //{
                //    Date = DateTime.Now.AddHours(1),
                //    BrandName = "ZAZ",
                //    Price = 333
                //});
                //doc1.Cars.Add(new BinaryCar
                //{
                //    Date = DateTime.Now.AddHours(1),
                //    BrandName = "WRANGLER",
                //    Price = 444
                //});


                //binConv.Save(read);
                //read = binConv.Read(read.Keys.ToArray());


            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
