using CommonFileConverter.Converters;
using CommonFileConverter.Serializers;
using XmlBinFileConverter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XmlBinFileConverter.Converters;
using System.ComponentModel.DataAnnotations;
using CommonFileConverter.Exceptions;
using XmlBinFileConverter.Constants;

namespace XmlBinFileConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string path = @"C:\Temp";

                BinaryFileConverter binConv = new BinaryFileConverter();
                XmlFileConverter xmlConv = new XmlFileConverter();

                CarsCollection<XmlCar> cars = new CarsCollection<XmlCar>();
                cars.Add(new XmlCar
                {
                    Date = DateTime.Now,
                    BrandName = "Alpha Romeo Brera",
                    Price = 60000,
                });
                cars.Add(new XmlCar
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandName = "Mazda",
                    Price = 25000
                });

                XmlBasedFileStructure file = new XmlBasedFileStructure(cars);
                Dictionary<string, XmlBasedFileStructure> files = new Dictionary<string, XmlBasedFileStructure>();

                for (int i = 0; i < 10; i++)
                {
                    files.Add(Path.Combine(path, String.Format("{0}.cxml", i)), file);
                }

                xmlConv.Save(files);
                IDictionary<string, XmlBasedFileStructure> xmlFiles = xmlConv.Read(files.Keys.ToArray());
                XmlBasedFileStructure file_0 = xmlFiles[Path.Combine(path, String.Format("{0}.cxml", 0))];
                file_0.Cars["Mazda"].Price = 22000;
                file_0.Cars["Mazda"].Date = DateTime.Now.AddMonths(1);
                file_0.Cars.Remove("Alpha Romeo Brera");

                xmlConv.Save(xmlFiles);

                xmlFiles = xmlConv.Read(files.Keys.ToArray());
                XmlBasedFileStructure file_1 = xmlFiles[Path.Combine(path, String.Format("{0}.cxml", 1))];
                file_1.Cars.Add(new XmlCar
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandName = "BMW",
                    Price = 40000
                });
                file_1.Cars.Add(new XmlCar
                {
                    Date = DateTime.Now.AddHours(2),
                    BrandName = "ZAZ",
                    Price = 300
                });
                file_1.Cars.Add(new XmlCar
                {
                    Date = DateTime.Now.AddHours(-3),
                    BrandName = "WRANGLER",
                    Price = 60000
                });

                xmlConv.Save(xmlFiles);

                xmlFiles = xmlConv.Read(files.Keys.ToArray());
                IDictionary<string, BinaryBasedFileStructure> binaryFiles = xmlConv.Convert<BinaryBasedFileStructure>(xmlFiles);

                try
                {
                    binConv.Save(binaryFiles);
                }
                catch (AggregateException ex)
                {
                    ex.Handle((inner) =>
                    {
                        if (inner is ModifyFileException) // each file saving operation throws exception
                        {
                            AggregateException agEx = inner.InnerException as AggregateException;
                            if (agEx != null)
                            {
                                agEx.Handle((innerException) => /// several validation errors
                                {
                                    if (innerException is ValidationException && innerException.Message.Contains(XmlBinMessages.StringExceedsMaxLenght))
                                    {
                                        return true;
                                    }
                                    return false;
                                });
                                return true;
                            }
                        }
                        return false; 
                    });

                    foreach (var pathFile in binaryFiles)
                    {
                        foreach (var car in pathFile.Value.Cars)
                        {
                            car.BrandName = car.BrandName.Substring(0, 2);
                        }
                    }
                }

                binConv.Save(binaryFiles);
                xmlConv.Delete(files.Keys.ToArray());

                binaryFiles = binConv.Read(binaryFiles.Keys.ToArray());
                BinaryBasedFileStructure file_2 = binaryFiles[Path.Combine(path, String.Format("{0}.cbin", 2))];
                file_2.Cars[0].Price = 21000;
                file_2.Cars.RemoveAt(1);
                binConv.Save(binaryFiles);
                binaryFiles = binConv.Read(binaryFiles.Keys.ToArray());
                binConv.Delete(files.Keys.ToArray());

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
