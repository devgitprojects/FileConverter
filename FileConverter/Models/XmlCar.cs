using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using CommonFileConverter.Models;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using XmlBinFileConverter.Constants;

namespace XmlBinFileConverter.Models
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public class XmlCar : BaseFileStructure, IXmlSerializable, IConvertible<XmlCar>, IInitializable<BinaryCar>
    {
        const string xmlDateFormat = "dd.MM.yyyy";

        public XmlCar() { }

        public DateTime Date { get; set; }
        public virtual string BrandName { get; set; }
        public uint Price { get; set; }

        #region IXmlSerializable

        public XmlSchema GetSchema()
        {
            return (null);
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            Date = DateTime.ParseExact(reader.ReadElementString(XmlBinMessages.FileStructureFields.CarFields.Date), xmlDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            BrandName = reader.ReadElementString(XmlBinMessages.FileStructureFields.CarFields.BrandName);
            Price = Convert.ToUInt32(reader.ReadElementString(XmlBinMessages.FileStructureFields.CarFields.Price));
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(XmlBinMessages.FileStructureFields.CarFields.Date, this.Date.ToString(xmlDateFormat));
            writer.WriteElementString(XmlBinMessages.FileStructureFields.CarFields.BrandName, this.BrandName);
            writer.WriteElementString(XmlBinMessages.FileStructureFields.CarFields.Price, this.Price.ToString());
        }

        #endregion

        #region IConvertible<XmlCar>

        TTo IConvertible<XmlCar>.Convert<TTo>(Mapper<XmlCar, TTo> mapper)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IInitializable<BinaryCar>

        void IInitializable<BinaryCar>.Initialize(BinaryCar from)
        {
            from.ThrowArgumentNullExceptionIfNull();
            BrandName = from.BrandName;
            Date = from.Date;
            Price = from.Price;
        }

        #endregion
    }

}
