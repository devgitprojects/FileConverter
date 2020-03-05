using FileConverter.Constants;
using FileConverter.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    public class BinaryBasedFileStructure : BaseFileStructure, ISerializable
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
        public uint RecordsCount { get; set; }
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
            this.Cars.Validate();
        }
    }
}
