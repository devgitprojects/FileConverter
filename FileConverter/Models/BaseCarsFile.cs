using CommonFileConverter.Extensions;
using CommonFileConverter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using XmlBinFileConverter.Constants;

namespace XmlBinFileConverter.Models
{
    [Serializable]
    public abstract class BaseCarsFile<TCar> : BaseModel, IEquatable<BaseCarsFile<TCar>>
        where TCar : XmlCar
    {
        public BaseCarsFile() { }
        public BaseCarsFile(CarsCollection<TCar> cars)
        {
            cars.ThrowArgumentNullExceptionIfNull();
            carsField = cars;
        }

        [XmlElement(XmlBinMessages.FileStructureFields.Car)]
        [Required(ErrorMessage = XmlBinMessages.CollectionShouldContainZeroOrMoreItems + XmlBinMessages.FileStructureFields.Cars)]
        public virtual CarsCollection<TCar> Cars 
        { 
            get { return carsField; }
            set { carsField = value; }
        }

        public override void Validate()
        {
            base.Validate();
            Cars.ThrowArgumentNullExceptionIfNull();
            Cars.Validate();
        }

        #region IEquatable<BinaryCarsFile>

        public override int GetHashCode()
        {
            return -656080538 + EqualityComparer<CarsCollection<TCar>>.Default.GetHashCode(Cars);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseCarsFile<TCar>);
        }

        public bool Equals(BaseCarsFile<TCar> other)
        {
            return other != null
                && ((Cars == null && other.Cars == null) || (Cars != null && other.Cars != null && Enumerable.SequenceEqual(Cars, other.Cars)));
        }

        #endregion

        private CarsCollection<TCar> carsField;
    }
}
