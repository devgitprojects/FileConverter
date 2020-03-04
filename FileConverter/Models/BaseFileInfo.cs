using System;
using System.Runtime.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    public class BaseFileInfo : ISerializable
    {
        public DateTime Date { get; set; }
        public string BrandName { get; set; }
        public int Price { get; set; }

        public BaseFileInfo() { }

        public BaseFileInfo(SerializationInfo info, StreamingContext context)
        {
            Date = (DateTime)info.GetValue("Date", typeof(DateTime));
            BrandName = (string)info.GetValue("BrandName", typeof(string));
            Price = (int)info.GetValue("Price", typeof(int));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Date", this.Date, typeof(DateTime));
            info.AddValue("BrandName", this.BrandName, typeof(string));
            info.AddValue("Price", this.Price, typeof(int));
        }
    }
}
