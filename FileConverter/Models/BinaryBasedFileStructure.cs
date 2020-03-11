using CommonFileConverter.Constants;
using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using CommonFileConverter.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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
            Cars = (CarsCollection<BinaryCar>)info.GetValue("Cars", typeof(CarsCollection<BinaryCar>));
            Header = info.GetInt16("Header");
            RecordsCount = info.GetUInt32("RecordsCount");
        }

        public short Header { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = LogMessages.ValueShouldBePositive + "RecordsCount")]
        public uint RecordsCount { get; private set; }
        [Required(ErrorMessage = LogMessages.CollectionShouldContainZeroOrMoreItems + "Cars")]
        public CarsCollection<BinaryCar> Cars { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Cars", this.Cars, typeof(CarsCollection<BinaryCar>));
            info.AddValue("Header", this.Header);
            info.AddValue("RecordsCount", this.Cars.Count);
        }

        public override void Validate()
        {
            base.Validate();
            Cars.ThrowArgumentNullExceptionIfNull();
            Cars.Validate();
        }

        TTo IConvertible<BinaryBasedFileStructure>.Convert<TTo>(Mapper<BinaryBasedFileStructure, TTo> mapper)
        {
            mapper.ThrowArgumentNullExceptionIfNull();
            return mapper.Convert(this);
        }

        void IInitializable<XmlBasedFileStructure>.Initialize(XmlBasedFileStructure from)
        {
            from.ThrowArgumentNullExceptionIfNull();
            Cars = new CarsCollection<BinaryCar>(from.Cars.Convert(new Mapper<XmlCar, BinaryCar>()));
            Header = 0x2526;
            RecordsCount = (uint)Cars.Count;
        }
    }
}
