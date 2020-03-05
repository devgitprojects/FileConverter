using FileConverter.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    public class XmlCar : BaseFileStructure, IXmlSerializable
    {
        const string xmlDateFormat = "dd.MM.yyyy";

        public XmlCar() { }

        public DateTime Date { get; set; }
        public string BrandName { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = LogMessages.ValueShouldBePositive + "Price")]
        public int Price { get; set; }

        public XmlSchema GetSchema()
        {
            return(null);
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            Date = DateTime.ParseExact(reader.ReadElementString("Date"), xmlDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            BrandName = reader.ReadElementString("BrandName");
            Price = Convert.ToInt32(reader.ReadElementString("Price"));
            reader.ReadEndElement();            
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Date", this.Date.ToString(xmlDateFormat));
            writer.WriteElementString("BrandName", this.BrandName);
            writer.WriteElementString("Price", this.Price.ToString());
        }
    }
}
