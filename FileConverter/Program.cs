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
                //BaseConverter<LinkedList<BinaryBasedFileStructure>> binConv = new BaseConverter<LinkedList<BinaryBasedFileStructure>>(new BinaryBasedSerializer<LinkedList<BinaryBasedFileStructure>>());

                string path = @"C:\Temp";

                LinkedList<Car> fileContent = new LinkedList<Car>();
                fileContent.AddLast(
                    new Car
                    {
                        Date = DateTime.Now,
                        BrandNameLength = 6,
                        BrandName = "Nissan",
                        Price = 1,

                    });

                fileContent.AddLast(new Car
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandNameLength = 5,
                    BrandName = "Mazda",
                    Price = 2
                });

                XmlBasedFileStructure file = new XmlBasedFileStructure(fileContent);
                Dictionary<string, XmlBasedFileStructure> files = new Dictionary<string, XmlBasedFileStructure>();

                for (int i = 0; i < 1; i++)
                {
                    files.Add(Path.Combine(path, String.Format("{0}.cxml", i)), file);
                }

                xmlConv.Save(files);

                var read = xmlConv.Read(files.Keys.ToArray());

                var fileRead = read.FirstOrDefault();
                var doc1 = fileRead.Value;
                doc1.Cars.Last.Value.BrandName = "SUZUKI";
                doc1.Cars.RemoveFirst();

                xmlConv.Save(read);

                read = xmlConv.Read(read.Keys.ToArray());

                doc1 = read.FirstOrDefault().Value;
                doc1.Cars.AddLast(new Car
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandNameLength = 3,
                    BrandName = "BMW",
                    Price = 22
                });
                doc1.Cars.AddLast(new Car
                {
                    Date = DateTime.Now.AddHours(1),
                    BrandNameLength = 3,
                    BrandName = "ZAZ",
                    Price = 333
                });
                doc1.Cars.AddLast(new Car
                {                   
                    Date = DateTime.Now.AddHours(1),
                    BrandNameLength = 3,
                    BrandName = "WRANGLER",
                    Price = 444
                });


                xmlConv.Save(read);
                read = xmlConv.Read(read.Keys.ToArray());


                //Document doc = new Document(fileContent);
                //Dictionary<string, Document> files = new Dictionary<string, Document>();

                //for (int i = 0; i < 10; i++)
                //{
                //    files.Add(Path.Combine(path, String.Format("{0}.cxml", i)), doc);
                //}

                //xmlConv.Save(files);

                //var read = xmlConv.Read(files.Keys.ToArray());

                //var file = read.FirstOrDefault();
                //var doc1 = file.Value as Document;
                //doc1.Cars[1].BrandName = "SUZUKI";
                //doc1.Cars.RemoveAt(0);

                ////var toSave = new Dictionary<string, ISerializable>();
                ////toSave.Add(file.Key, file.Value);
                //xmlConv.Save(read);

                //read = xmlConv.Read(read.Keys.ToArray());
                //doc1 = read.FirstOrDefault().Value as Document;
                //doc1.Cars.Add(new XmlBasedFileInfo
                //{
                //    BrandName = "BMW",
                //    Date = DateTime.Now.AddHours(1),
                //    Price = 22
                //});
                //doc1.Cars.Add(new XmlBasedFileInfo
                //{
                //    BrandName = "ZAZ",
                //    Date = DateTime.Now.AddHours(1),
                //    Price = 333
                //});
                //doc1.Cars.Add(new XmlBasedFileInfo
                //{
                //    BrandName = "WRANGLER",
                //    Date = DateTime.Now.AddHours(1),
                //    Price = 444
                //});


                //xmlConv.Save(read);
                //xmlConv.Delete(files.Keys.ToArray());

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
