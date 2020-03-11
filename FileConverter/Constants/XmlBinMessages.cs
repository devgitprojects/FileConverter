using CommonFileConverter.Constants;

namespace XmlBinFileConverter.Constants
{
    public class XmlBinMessages : LogMessages
    {
        public class FileStructureFields
        {
            public const string Car = "Car";
            public const string Cars = "Cars";
            public const string Document = "Document";
            public const string Header = "Header";
            public const string RecordsCount = "RecordsCount";

            public class CarFields
            {
                public const string BrandNameLength = "BrandNameLength";
                public const string BrandName = "BrandName";
                public const string Date = "Date";
                public const string Price = "Price";
            }
        }
    }
}
