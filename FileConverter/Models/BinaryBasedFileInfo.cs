using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter.Models
{
    [Serializable]
    public class BinaryBasedFileInfo : BaseFileInfo
    {
        public short Header { get; set; }
        public uint RecordsCount { get; set; }
        public ushort BrandNameLength { get; set; }
    }
}
