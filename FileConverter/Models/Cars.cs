using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    public class Cars : SerializableKeyedCollection<string, Car>, ISerializable
    {
        public Cars() : base() { }
        public Cars(IEqualityComparer<string> comparer) : base(comparer) { }
        public Cars(IEqualityComparer<string> comparer, int dictionaryCreationThreshold) : base(comparer, dictionaryCreationThreshold) { }
        protected Cars(SerializationInfo info, StreamingContext context)
        {
            SetObjectData(info, context);
        }

        protected override string GetKeyForItem(Car item)
        {
            return item.BrandName;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            int index = 0;
            foreach (Car item in this)
            {
                info.AddValue(String.Format("Item_{0}", index++), item, typeof(Car));
            }
        }

        protected virtual void SetObjectData(SerializationInfo info, StreamingContext context)
        {
            int index = 0;
            foreach (SerializationEntry entry in info)
            {
                Car item = (Car)info.GetValue(String.Format("Item_{0}", index++), typeof(Car));
                this.Add(item);
            }
        }
    }
}
