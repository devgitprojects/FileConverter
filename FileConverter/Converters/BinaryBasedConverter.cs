using FileConverter.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileConverter.Converters
{
    internal class BinaryBasedConverter : BaseConverter<ISerializable>
    {
        internal BinaryBasedConverter(ISerializer serializer)
        {
            this.Serializer = serializer;
        }

        public override void Create(IDictionary<string, ISerializable> files)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, ISerializable> Read(params string[] filesPaths)
        {
            throw new NotImplementedException();
        }

        public override void Save(IDictionary<string, ISerializable> filesToSave)
        {
            throw new NotImplementedException();
        }

        protected override void Convert()
        {
            throw new NotImplementedException();
        }
    }
}
