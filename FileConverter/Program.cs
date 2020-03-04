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

                //BaseConverter<Document> xmlConv = new BaseConverter<Document>(new XmlBasedSerializer<Document>());
                BaseConverter<List<BinaryBasedFileInfo>> binConv = new BaseConverter<List<BinaryBasedFileInfo>>(new BinaryBasedSerializer());

                string path = @"C:\Temp";

                List<BinaryBasedFileInfo> fileContent = new List<BinaryBasedFileInfo>
                {
                    new BinaryBasedFileInfo
                    {
                        Header = 111,
                        RecordsCount = 2,
                        Date = DateTime.Now,
                        BrandNameLength = 6,
                        BrandName = "Nissan",
                        Price = 1,
                        
                    },
                    new BinaryBasedFileInfo
                    {
                        Header = 222,
                        RecordsCount = 2,
                        Date = DateTime.Now.AddHours(1),
                        BrandNameLength = 5,
                        BrandName = "Mazda",
                        Price = 2
                    }
                };

                Dictionary<string, List<BinaryBasedFileInfo>> files = new Dictionary<string, List<BinaryBasedFileInfo>>();

                for (int i = 0; i < 1; i++)
                {
                    files.Add(Path.Combine(path, String.Format("{0}.cbin", i)), fileContent);
                }

                binConv.Save(files);

                var read = binConv.Read(files.Keys.ToArray());

                var file = read.FirstOrDefault();
                var doc1 = file.Value as List<BinaryBasedFileInfo>;
                doc1[1].BrandName = "SUZUKI";
                doc1.RemoveAt(0);

                binConv.Save(read);

                read = binConv.Read(read.Keys.ToArray());
                doc1 = read.FirstOrDefault().Value as List<BinaryBasedFileInfo>;
                doc1.Add(new BinaryBasedFileInfo
                {
                    Header = 222,
                    RecordsCount = 2,
                    Date = DateTime.Now.AddHours(1),
                    BrandNameLength = 3,
                    BrandName = "BMW",
                    Price = 22
                });
                doc1.Add(new BinaryBasedFileInfo
                {
                    Header = 222,
                    RecordsCount = 2,
                    Date = DateTime.Now.AddHours(1),
                    BrandNameLength = 3,
                    BrandName = "ZAZ",
                    Price = 333
                });
                doc1.Add(new BinaryBasedFileInfo
                {
                    Header = 222,
                    RecordsCount = 2,
                    Date = DateTime.Now.AddHours(1),
                    BrandNameLength = 3,
                    BrandName = "WRANGLER",
                    Price = 444
                });


                binConv.Save(read);

                //string path = @"C:\Temp";

                //List<XmlBasedFileInfo> fileContent = new List<XmlBasedFileInfo>
                //{
                //    new XmlBasedFileInfo
                //    {
                //        BrandName = "Nissan",
                //        Date = DateTime.Now,
                //        Price = 1
                //    },
                //    new XmlBasedFileInfo
                //    {
                //        BrandName = "Mazda",
                //        Date = DateTime.Now.AddHours(1),
                //        Price = 2
                //    }
                //};

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
                // xmlConv.Delete(files.Keys.ToArray());

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
