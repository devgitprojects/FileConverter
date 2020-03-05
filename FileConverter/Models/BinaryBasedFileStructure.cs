using FileConverter.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    public class BinaryBasedFileStructure : XmlBasedFileStructure, ISerializable
    {
        public BinaryBasedFileStructure() : base() { }
        public BinaryBasedFileStructure(Cars cars) : base(cars) { }
        protected BinaryBasedFileStructure(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Header = info.GetInt16("Header");
            RecordsCount = info.GetUInt32("RecordsCount");
        }

        public short Header { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = LogMessages.ValueShouldBePositive + "RecordsCount")]
        public uint RecordsCount { get; set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Header", this.Header);
            info.AddValue("RecordsCount", this.Cars.Count);
        }
    }
}
