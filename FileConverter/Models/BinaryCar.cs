using CommonFileConverter.Constants;
using CommonFileConverter.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;
using XmlBinFileConverter.Constants;

namespace XmlBinFileConverter.Models
{
    [Serializable]
    public class BinaryCar : XmlCar, ISerializable, IInitializable<XmlCar>
    {
        const string binaryDateFormat = "ddMMyyyy";

        public BinaryCar() { }

        protected BinaryCar(SerializationInfo info, StreamingContext context)
        {
            Date = DateTime.ParseExact(info.GetString(XmlBinMessages.FileStructureFields.CarFields.Date), binaryDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            BrandNameLength = info.GetUInt16(XmlBinMessages.FileStructureFields.CarFields.BrandNameLength);
            BrandName = info.GetString(XmlBinMessages.FileStructureFields.CarFields.BrandName);
            Price = info.GetInt32(XmlBinMessages.FileStructureFields.CarFields.Price);
        }

        [StringLength(2, ErrorMessage = LogMessages.StringExceedsMaxLenght + XmlBinMessages.FileStructureFields.CarFields.BrandName)]
        public override string BrandName { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = LogMessages.ValueShouldBePositive + XmlBinMessages.FileStructureFields.CarFields.BrandNameLength)]
        public ushort BrandNameLength { get; private set; }

        #region ISerializable

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(XmlBinMessages.FileStructureFields.CarFields.Date, Date.ToString(binaryDateFormat));
            info.AddValue(XmlBinMessages.FileStructureFields.CarFields.BrandNameLength, String.IsNullOrEmpty(BrandName) ? 0 : BrandName.Length);
            info.AddValue(XmlBinMessages.FileStructureFields.CarFields.BrandName, BrandName);
            info.AddValue(XmlBinMessages.FileStructureFields.CarFields.Price, Price);
        }

        #endregion

        #region  IInitializable<XmlCar>

        void IInitializable<XmlCar>.Initialize(XmlCar from)
        {
            this.BrandName = from.BrandName;
            this.Date = from.Date;
            this.Price = from.Price;
        }

        #endregion
    }
}
