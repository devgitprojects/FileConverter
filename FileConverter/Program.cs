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

                XmlBasedConverter xmlConv = new XmlBasedConverter();
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

                for (int i = 0; i < 100; i++)
                {
                    files.Add(Path.Combine(path, String.Format("{0}.cxml", i)), doc);
                }

                xmlConv.Create(files);


                var read = xmlConv.Read(files.Keys.ToArray());


                //xmlConv.Delete(files.Keys.ToArray());

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
