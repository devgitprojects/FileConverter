using CommonFileConverter.Constants;
using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using CommonFileConverter.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using XmlBinFileConverter.Constants;

namespace XmlBinFileConverter.Models
{
    [Serializable]
    public class BinaryBasedFileStructure : BaseFileStructure, ISerializable, IInitializable<XmlBasedFileStructure>, IConvertible<BinaryBasedFileStructure>
    {
        public BinaryBasedFileStructure() : base() { }
        public BinaryBasedFileStructure(CarsCollection<BinaryCar> cars)
        {
            cars.ThrowArgumentNullExceptionIfNull();
            Cars = cars;
        }
        protected BinaryBasedFileStructure(SerializationInfo info, StreamingContext context)
        {
            Cars = (CarsCollection<BinaryCar>)info.GetValue(XmlBinMessages.FileStructureFields.Cars, typeof(CarsCollection<BinaryCar>));
            Header = info.GetInt16(XmlBinMessages.FileStructureFields.Header);
            RecordsCount = info.GetUInt32(XmlBinMessages.FileStructureFields.RecordsCount);
        }

        public short Header { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = LogMessages.ValueShouldBePositive + XmlBinMessages.FileStructureFields.RecordsCount)]
        public uint RecordsCount { get; private set; }
        [Required(ErrorMessage = LogMessages.CollectionShouldContainZeroOrMoreItems + XmlBinMessages.FileStructureFields.Cars)]
        public CarsCollection<BinaryCar> Cars { get; set; }

        public override void Validate()
        {
            base.Validate();
            Cars.ThrowArgumentNullExceptionIfNull();
            Cars.Validate();
        }

        #region  IConvertible<BinaryBasedFileStructure>

        TTo IConvertible<BinaryBasedFileStructure>.Convert<TTo>(Mapper<BinaryBasedFileStructure, TTo> mapper)
        {
            mapper.ThrowArgumentNullExceptionIfNull();
            return mapper.Convert(this);
        }

        #endregion

        #region ISerializable

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(XmlBinMessages.FileStructureFields.Cars, Cars, typeof(CarsCollection<BinaryCar>));
            info.AddValue(XmlBinMessages.FileStructureFields.Header, Header);
            info.AddValue(XmlBinMessages.FileStructureFields.RecordsCount, Cars.Count);
        }

        #endregion

        #region  IInitializable<XmlBasedFileStructure>

        void IInitializable<XmlBasedFileStructure>.Initialize(XmlBasedFileStructure from)
        {
            from.ThrowArgumentNullExceptionIfNull();
            Cars = new CarsCollection<BinaryCar>(from.Cars.Convert(new Mapper<XmlCar, BinaryCar>()));
            Header = 0x2526;
            RecordsCount = (uint)Cars.Count;
        }

        #endregion
    }
}
