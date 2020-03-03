using FileConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter.Converters
{
    class BinaryBasedConverter //: BaseConverter<BinaryBasedFileInfo>
    {
        BinaryBasedConverter()
        {
        }

        private BinaryFormatter Serializer { get; set; }       
    }
}
