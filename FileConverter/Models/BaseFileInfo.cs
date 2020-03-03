using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter.Models
{
    [Serializable]
    public class BaseFileInfo
    {
        public DateTime Date { get; set; }
        public string BrandName { get; set; }
        public int Price { get; set; } 
    }
}
