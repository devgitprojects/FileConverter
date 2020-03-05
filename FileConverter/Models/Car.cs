using FileConverter.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    public class Car : BaseFileStructure, ISerializable
    {
        public Car() { }
        protected Car(SerializationInfo info, StreamingContext context)
        {
            Date = info.GetDateTime("Date");
            BrandNameLength = info.GetUInt16("BrandNameLength");
            BrandName = info.GetString("BrandName");
            Price = info.GetInt32("Price");
        }

        public DateTime Date { get; set; }
        [XmlIgnore]
        [Range(0, int.MaxValue, ErrorMessage = LogMessages.ValueShouldBePositive + "BrandNameLength")]
        public ushort BrandNameLength { get; set; }
        public string BrandName { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = LogMessages.ValueShouldBePositive + "Price")]
        public int Price { get; set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.Validate();
            info.AddValue("Date", this.Date);
            info.AddValue("BrandNameLength", this.BrandNameLength);
            info.AddValue("BrandName", this.BrandName);
            info.AddValue("Price", this.Price);
        }
    }
}
