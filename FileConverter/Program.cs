﻿using FileConverter.Converters;
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

                //BaseConverter<XmlBasedFileStructure> xmlConv = new BaseConverter<XmlBasedFileStructure>(new XmlBasedSerializer<XmlBasedFileStructure>());
                ////BaseConverter<LinkedList<BinaryBasedFileStructure>> binConv = new BaseConverter<LinkedList<BinaryBasedFileStructure>>(new BinaryBasedSerializer<LinkedList<BinaryBasedFileStructure>>());

                //string path = @"C:\Temp";

                //Cars cars = new Cars();
                //cars.Add(
                //    new Car
                //    {
                //        Date = DateTime.Now,
                //        BrandNameLength = 6,
                //        BrandName = "Nissan",
                //        Price = 1,

                //    });

                //cars.Add(new Car
                //{
                //    Date = DateTime.Now.AddHours(1),
                //    BrandNameLength = 5,
                //    BrandName = "Mazda",
                //    Price = 2
                //});

                //XmlBasedFileStructure file = new XmlBasedFileStructure(cars);
                //Dictionary<string, XmlBasedFileStructure> files = new Dictionary<string, XmlBasedFileStructure>();

                //for (int i = 0; i < 10; i++)
                //{
                //    files.Add(Path.Combine(path, String.Format("{0}.cxml", i)), file);
                //}

                //xmlConv.Save(files);

                //var read = xmlConv.Read(files.Keys.ToArray());

                //var fileRead = read.FirstOrDefault();
                //var doc1 = fileRead.Value;
                //doc1.Cars["Mazda"].BrandName = "SUZUKI";
                //doc1.Cars.Remove("Nissan");

                //xmlConv.Save(read);

                //read = xmlConv.Read(read.Keys.ToArray());

                //doc1 = read.FirstOrDefault().Value;
                //doc1.Cars.Add(new Car
                //{
                //    Date = DateTime.Now.AddHours(1),
                //    BrandNameLength = 3,
                //    BrandName = "BMW",
                //    Price = 22
                //});
                //doc1.Cars.Add(new Car
                //{
                //    Date = DateTime.Now.AddHours(1),
                //    BrandNameLength = 3,
                //    BrandName = "ZAZ",
                //    Price = 333
                //});
                //doc1.Cars.Add(new Car
                //{                   
                //    Date = DateTime.Now.AddHours(1),
                //    BrandNameLength = 3,
                //    BrandName = "WRANGLER",
                //    Price = 444
                //});


                //xmlConv.Save(read);
                //read = xmlConv.Read(read.Keys.ToArray());

                BaseConverter<BinaryBasedFileStructure> binConv = new BaseConverter<BinaryBasedFileStructure>(new BinaryBasedSerializer<BinaryBasedFileStructure>());

                string path = @"C:\Temp";

                Cars cars = new Cars();
                cars.Add(
                    new Car
                    {
                        Date = DateTime.Now,
                        BrandNameLength = 6,
                        BrandName = "Nissan",
                        Price = 1,

                    });

                cars.Add(new Car
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandNameLength = 5,
                    BrandName = "Mazda",
                    Price = 2
                });

                BinaryBasedFileStructure file = new BinaryBasedFileStructure(cars);
                Dictionary<string, BinaryBasedFileStructure> files = new Dictionary<string, BinaryBasedFileStructure>();


                for (int i = 0; i < 1; i++)
                {
                    files.Add(Path.Combine(path, String.Format("{0}.cxml", i)), file);
                }

                binConv.Save(files);

                var read = binConv.Read(files.Keys.ToArray());

                var fileRead = read.FirstOrDefault();
                var doc1 = fileRead.Value;
                doc1.Cars["Mazda"].BrandName = "SUZUKI";
                doc1.Cars.Remove("Nissan");

                binConv.Save(read);

                read = binConv.Read(read.Keys.ToArray());

                doc1 = read.FirstOrDefault().Value;
                doc1.Cars.Add(new Car
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandNameLength = 3,
                    BrandName = "BMW",
                    Price = 22
                });
                doc1.Cars.Add(new Car
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandNameLength = 3,
                    BrandName = "ZAZ",
                    Price = 333
                });
                doc1.Cars.Add(new Car
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandNameLength = 3,
                    BrandName = "WRANGLER",
                    Price = 444
                });


                binConv.Save(read);
                read = binConv.Read(read.Keys.ToArray());


            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
