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
    public class BinaryCar : XmlCar, IInitializable<XmlCar>, IEquatable<BinaryCar>
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

        #region IEquatable<BinaryCar>

        public override int GetHashCode()
        {
            var hashCode = -930785528;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + BrandNameLength.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BrandName);
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BinaryCar);
        }

        public bool Equals(BinaryCar other)
        {
            return base.Equals(other) && BrandNameLength == other.BrandNameLength;
        }

        #endregion

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
