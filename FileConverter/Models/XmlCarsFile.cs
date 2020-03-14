using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using System;
using System.Xml.Serialization;
using XmlBinFileConverter.Constants;

namespace XmlBinFileConverter.Models
{
    [Serializable]
    [XmlRoot(XmlBinMessages.FileStructureFields.Document)]
    public class XmlCarsFile : BaseCarsFile<XmlCar>, IInitializable<BinaryCarsFile>, IConvertible<XmlCarsFile>
    {
        public XmlCarsFile() { }
        public XmlCarsFile(CarsCollection<XmlCar> cars) : base(cars) { }

        #region  IConvertible<XmlBasedFileStructure>

        TTo IConvertible<XmlCarsFile>.Convert<TTo>(Mapper<XmlCarsFile, TTo> mapper)
        {
            return Convert(mapper);
        }

        protected virtual TTo Convert<TTo>(Mapper<XmlCarsFile, TTo> mapper)
            where TTo : IInitializable<XmlCarsFile>, new()
        {
            mapper.ThrowArgumentNullExceptionIfNull();
            return mapper.Convert(this);
        }

        #endregion

        #region  IInitializable<BinaryBasedFileStructure>

        void IInitializable<BinaryCarsFile>.Initialize(BinaryCarsFile from)
        {
            Initialize(from);
        }

        protected virtual void Initialize(BinaryCarsFile from)
        {
            from.ThrowArgumentNullExceptionIfNull();
            Cars = new CarsCollection<XmlCar>(from.Cars.Convert(new Mapper<BinaryCar, XmlCar>()));
        }

        #endregion
    }
}
