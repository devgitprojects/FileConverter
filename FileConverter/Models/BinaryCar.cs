using CommonFileConverter.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using XmlBinFileConverter.Constants;

namespace XmlBinFileConverter.Models
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public class BinaryCar : XmlCar, IInitializable<XmlCar>
    {
        const string binaryDateFormat = "ddMMyyyy";
        const int maxLenghtOfunicodeBrandName = 2;

        public BinaryCar() { }

        public ushort BrandNameLength { get; internal set; }
        [StringLength(maxLenghtOfunicodeBrandName, ErrorMessage = XmlBinMessages.StringExceedsMaxLenght + XmlBinMessages.FileStructureFields.CarFields.BrandName)]
        public override string BrandName
        {
            get
            {
                return base.BrandName;
            }
            set
            {
                base.BrandName = value;
                BrandNameLength = (ushort)(string.IsNullOrEmpty(value) ? 0 : value.Length); ///will be validate during serialization
            }
        }

        public override bool Validate(out List<ValidationResult> errors)
        {
            bool isValid = base.Validate(out errors);

            ushort brandNameLenght = (ushort)(String.IsNullOrEmpty(BrandName) ? 0 : BrandName.Length);
            if (brandNameLenght != BrandNameLength)
            {
                isValid = false;
                errors.Add(new ValidationResult(String.Format(XmlBinMessages.ValuesAreNotEqual,
                    XmlBinMessages.FileStructureFields.CarFields.BrandName, brandNameLenght, XmlBinMessages.FileStructureFields.CarFields.BrandNameLength, BrandNameLength)));
            }

            return isValid;
        }

        #region  IInitializable<XmlCar>

        void IInitializable<XmlCar>.Initialize(XmlCar from)
        {
            this.BrandName = from.BrandName;
            this.Date = from.Date;
            this.Price = from.Price;
        }

        #endregion
    }
}
