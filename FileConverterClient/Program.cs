using CommonFileConverter.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using XmlBinFileConverter.Constants;
using XmlBinFileConverter.Converters;
using XmlBinFileConverter.Models;

namespace XmlBinFileConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetTempPath();
            BinaryFileConverter binConv = new BinaryFileConverter();
            XmlFileConverter xmlConv = new XmlFileConverter();

            Console.WriteLine("Application will create XML files, read and modify them and convert to BIN files. The same operations will be done with BIN files.");
            Console.WriteLine("Press Enter to start.");
            Console.ReadLine();

            Console.WriteLine("Generates files dictionary where key is path and value is instance of file model.");

            Dictionary<string, XmlCarsFile> files = GenerateXMLFilesDictionary(3);

            Console.WriteLine("3 files in dictionary: {0}", String.Join(Environment.NewLine, files.Keys.ToArray()));

            Console.WriteLine("Saves files on disk");

            xmlConv.Save(files);

            Console.WriteLine("Reads saved files");

            IDictionary<string, XmlCarsFile> xmlFiles = xmlConv.Read(files.Keys.ToArray());

            Console.WriteLine("Gets and modifies file_0");

            XmlCarsFile file_0 = xmlFiles[Path.Combine(path, String.Format("{0}.cxml", 0))];
                
            file_0.Cars["Mazda"].Price = 22000;
            file_0.Cars["Mazda"].Date = DateTime.Now.AddMonths(1);
            file_0.Cars.Remove("Alpha Romeo Brera");


            Console.WriteLine("Saves modified file");

            xmlConv.Save(xmlFiles);

            Console.WriteLine("Reads and modifies file_1");

            xmlFiles = xmlConv.Read(files.Keys.ToArray());
                
            XmlCarsFile file_1 = xmlFiles[Path.Combine(path, String.Format("{0}.cxml", 1))];
            file_1.Cars.Add(new XmlCar(DateTime.Now.AddDays(1), "BMW", 40000));
            file_1.Cars.Add(new XmlCar(DateTime.Now.AddMonths(2), "ZAZ", 300));
            file_1.Cars.Add(new XmlCar(DateTime.Now.AddMonths(-3), "WRANGLER", 60000));

            Console.WriteLine("Saves modified file");

            xmlConv.Save(xmlFiles);

            Console.WriteLine("Reads XML files and converts them to BIN files");

            xmlFiles = xmlConv.Read(files.Keys.ToArray());
            IDictionary<string, BinaryCarsFile> binaryFiles = xmlConv.Convert<BinaryCarsFile>(xmlFiles);

            try
            {
                Console.WriteLine("Attempt to save BIN files."); 
                binConv.Save(binaryFiles);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("each file saving task throws ModifyFileException exception which could contain several validation exceptions"); 
                HandleAggregateException(ex);

                Console.WriteLine("fix validation errors");                
                foreach (var pathFile in binaryFiles)
                {
                    foreach (var car in pathFile.Value.Cars)
                    {
                        car.BrandName = car.BrandName.Substring(0, 2);
                    }
                }
            }

            Console.WriteLine("Saves BIN files");
            
            binConv.Save(binaryFiles);

            Console.WriteLine("Reads and modifies BIN files");
          
            binaryFiles = binConv.Read(binaryFiles.Keys.ToArray());
                
            BinaryCarsFile file_2 = binaryFiles[Path.Combine(path, String.Format("{0}.cbin", 2))];
            file_2.Cars[0].Price = 21000;
            file_2.Cars[0].Date.AddMonths(13);
            file_2.Cars.RemoveAt(1);

            Console.WriteLine("Saves modified BIN files");
            
            binConv.Save(binaryFiles);
            binaryFiles = binConv.Read(binaryFiles.Keys.ToArray());

            if (AskForRemove() == ConsoleKey.Y)
            {
                xmlConv.Delete(xmlFiles.Keys.ToArray());
                binConv.Delete(binaryFiles.Keys.ToArray());
            }
        }

        static XmlCarsFile GenerateXMLFile(params XmlCar[] enumerable)
        {
            return new XmlCarsFile(new CarsCollection<XmlCar>(enumerable));
        }

        static Dictionary<string, XmlCarsFile> GenerateXMLFilesDictionary(int filesCount)
        {
            string path = Path.GetTempPath();
            Dictionary<string, XmlCarsFile> files = new Dictionary<string, XmlCarsFile>();

            for (int i = 0; i < filesCount; i++)
            {
                XmlCarsFile file = GenerateXMLFile(
                    new XmlCar(DateTime.Now, "Alpha Romeo Brera", 37000),
                    new XmlCar(DateTime.Now.AddDays(1), "Mazda", 25000));

                files.Add(Path.Combine(path, String.Format("{0}.cxml", i)), file);
            }

            return files;
        }

        static void HandleAggregateException(AggregateException aex)
        {
            aex.Handle((inner) =>
            {
                if (inner is ModifyFileException) // each file saving task throws ModifyFileException exception which could contain several validation exceptions
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
        }

        static ConsoleKey AskForRemove()
        {
            Console.WriteLine(@"Do you want to remove created files Y\N?");

            ConsoleKey key;
            while (true)
            {
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.Y || key == ConsoleKey.N)
                {
                    break;
                }
                Console.WriteLine(@"Wrong input. Enter Y\N");
            }

            return key;
        }
    }
}
