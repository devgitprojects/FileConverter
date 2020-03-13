using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using XmlBinFileConverter.Constants;

namespace XmlBinFileConverter.Models
{
    [Serializable]
    public class BinaryCarsFile : BaseCarsFile<BinaryCar>, IInitializable<XmlCarsFile>, IConvertible<BinaryCarsFile>, IEquatable<BinaryCarsFile>
    {
        const short headerValue = 0x2526;

        public BinaryCarsFile() { }
        public BinaryCarsFile(CarsCollection<BinaryCar> cars) : base(cars) { }

        [Range(headerValue, headerValue, ErrorMessage = XmlBinMessages.ValueNotInRange + XmlBinMessages.FileStructureFields.Header)]
        public short Header { get { return headerField; } }
        public uint RecordsCount { get; internal set; }
        public override CarsCollection<BinaryCar> Cars
        {
            get
            {
                return base.Cars;
            }
            set
            {
                if (base.Cars != null)
                {
                    base.Cars.CollectionChanged -= CarsCollectionChanged;
                    base.Cars.Clear();
                }

                base.Cars = value;
                if (base.Cars != null)
                {
                    base.Cars.CollectionChanged += CarsCollectionChanged;
                    CarsCollectionChanged(base.Cars, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }

        public override void Validate()
        {
            base.Validate();
            if (RecordsCount != Cars.Count)
            {
                throw new ValidationException(String.Format(XmlBinMessages.ValuesAreNotEqual,
                    XmlBinMessages.FileStructureFields.RecordsCount, RecordsCount, XmlBinMessages.FileStructureFields.Cars, Cars.Count));
            }
        }

        #region  IConvertible<BinaryBasedFileStructure>

        TTo IConvertible<BinaryCarsFile>.Convert<TTo>(Mapper<BinaryCarsFile, TTo> mapper)
        {
            mapper.ThrowArgumentNullExceptionIfNull();
            return mapper.Convert(this);
        }

        #endregion

        #region IEquatable<BinaryCarsFile>

        public override int GetHashCode()
        {
            var hashCode = 152924670;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + RecordsCount.GetHashCode();
            hashCode = hashCode * -1521134295 + headerField.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BinaryCarsFile);
        }

        public bool Equals(BinaryCarsFile other)
        {
            return other != null
                && base.Equals(other)
                && Header == other.Header
                && RecordsCount == other.RecordsCount;
        }

        #endregion

        #region  IInitializable<XmlBasedFileStructure>

        void IInitializable<XmlCarsFile>.Initialize(XmlCarsFile from)
        {
            from.ThrowArgumentNullExceptionIfNull();
            Cars = new CarsCollection<BinaryCar>(from.Cars.Convert(new Mapper<XmlCar, BinaryCar>()));
        }

        #endregion

        private void CarsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var cars = sender as ICollection;
            this.RecordsCount = (uint)(cars == null ? 0 : cars.Count); ///will be validate during serialization
        }

        internal short headerField = headerValue;
    }
}
