using System;
using System.Runtime.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    public class BinaryBasedFileInfo : BaseFileInfo, ISerializable
    {
        public short Header { get; set; }
        public uint RecordsCount { get; set; }
        public ushort BrandNameLength { get; set; }

        public BinaryBasedFileInfo(SerializationInfo info, StreamingContext context)
        {
            Header = (short)info.GetValue("Header", typeof(short));
            RecordsCount = (uint)info.GetValue("RecordsCount", typeof(uint));
            BrandNameLength = (ushort)info.GetValue("BrandNameLength", typeof(ushort));

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Header", this.Header, typeof(short));
            info.AddValue("RecordsCount", this.RecordsCount, typeof(uint));
            info.AddValue("BrandNameLength", this.BrandNameLength, typeof(ushort));
        }
    }
}
