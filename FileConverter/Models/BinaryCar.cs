using FileConverter.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    public class BinaryCar : XmlCar, ISerializable
    {
        const string binaryDateFormat = "ddMMyyyy";

        public BinaryCar() { }
        protected BinaryCar(SerializationInfo info, StreamingContext context)
        {
            Date = DateTime.ParseExact(info.GetString("Date"), binaryDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            BrandNameLength = info.GetUInt16("BrandNameLength");
            BrandName = info.GetString("BrandName");
            Price = info.GetInt32("Price");
        }

        [Range(0, int.MaxValue, ErrorMessage = LogMessages.ValueShouldBePositive + "BrandNameLength")]
        public ushort BrandNameLength { get; set; }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Date", this.Date.ToString(binaryDateFormat));
            info.AddValue("BrandNameLength", String.IsNullOrEmpty(this.BrandName) ? 0 : this.BrandName.Length);
            info.AddValue("BrandName", this.BrandName);
            info.AddValue("Price", this.Price);
        }
    }
}
