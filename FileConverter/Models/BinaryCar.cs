using FileConverter.Constants;
using FileConverter.Converters;
using FileConverter.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    public class BinaryCar : XmlCar, ISerializable, IConvertible<BinaryCar>, IInitializable<XmlCar>
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

        [StringLength(2, ErrorMessage = LogMessages.StringExceedsMaxLenght + "BrandName")]
        public override string BrandName { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = LogMessages.ValueShouldBePositive + "BrandNameLength")]
        public ushort BrandNameLength { get; private set; }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Date", Date.ToString(binaryDateFormat));
            info.AddValue("BrandNameLength", String.IsNullOrEmpty(BrandName) ? 0 : BrandName.Length);
            info.AddValue("BrandName", BrandName);
            info.AddValue("Price", Price);
        }

        TTo IConvertible<BinaryCar>.Convert<TTo>(Mapper<BinaryCar, TTo> mapper)
        {
            throw new NotImplementedException();
        }

        void IInitializable<XmlCar>.Initialize(XmlCar from)
        {
            this.BrandName = from.BrandName;
            this.Date = from.Date;
            this.Price = from.Price;
        }



        //XmlCar IConvertable<XmlCar>.Convert()
        //{
        //    return new XmlCar
        //    {
        //        BrandName = BrandName,
        //        Date = Date,
        //        Price = Price
        //    };
        //}
    }
}
