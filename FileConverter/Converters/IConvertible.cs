using FileConverter.Models;

namespace FileConverter.Converters
{
    public interface IInitializable<TFrom> where TFrom : BaseFileStructure
    {
        void Initialize(TFrom from);
    }

    public interface IConvertible<TFrom> 
        where TFrom : BaseFileStructure
    {
        TTo Convert<TTo>(Mapper<TFrom, TTo> mapper) where TTo : IInitializable<TFrom>, new();
    }    

    public class Mapper<TFrom, TTo> 
        where TFrom : BaseFileStructure
        where TTo : IInitializable<TFrom>, new()
    {
        public TTo Convert(TFrom from)
        {
            var converted = new TTo();
            converted.Initialize(from);
            return converted;
        }
    }
}
