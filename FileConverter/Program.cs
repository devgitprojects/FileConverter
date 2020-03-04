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

                Converter xmlConv = new Converter( new XmlBasedSerializer<Document>());
                string path = @"C:\Temp";
                
                List<XmlBasedFileInfo> fileContent = new List<XmlBasedFileInfo>
                {
                    new XmlBasedFileInfo
                    {
                        BrandName = "Nissan",
                        Date = DateTime.Now,
                        Price = 1
                    },
                    new XmlBasedFileInfo
                    {
                        BrandName = "Mazda",
                        Date = DateTime.Now.AddHours(1),
                        Price = 2
                    }
                };

                Document doc = new Document(fileContent);
                Dictionary<string, ISerializable> files = new Dictionary<string, ISerializable>();

                for (int i = 0; i < 10; i++)
                {
                    files.Add(Path.Combine(path, String.Format("{0}.cxml", i)), doc);
                }
               
                xmlConv.Save(files);

                var read = xmlConv.Read(files.Keys.ToArray());

                var file = read.FirstOrDefault();
                var doc1 = file.Value as Document;
                doc1.Cars[1].BrandName = "SUZUKI";
                doc1.Cars.RemoveAt(0);

                //var toSave = new Dictionary<string, ISerializable>();
                //toSave.Add(file.Key, file.Value);
                xmlConv.Save(read);

                read = xmlConv.Read(read.Keys.ToArray());
                doc1 = read.FirstOrDefault().Value as Document;
                doc1.Cars.Add(new XmlBasedFileInfo
                {
                    BrandName = "BMW",
                    Date = DateTime.Now.AddHours(1),
                    Price = 22
                });
                doc1.Cars.Add(new XmlBasedFileInfo
                {
                    BrandName = "ZAZ",
                    Date = DateTime.Now.AddHours(1),
                    Price = 333
                });
                doc1.Cars.Add(new XmlBasedFileInfo
                {
                    BrandName = "WRANGLER",
                    Date = DateTime.Now.AddHours(1),
                    Price = 444
                });


                xmlConv.Save(read);
                // xmlConv.Delete(files.Keys.ToArray());

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
