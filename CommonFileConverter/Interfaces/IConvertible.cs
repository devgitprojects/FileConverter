using CommonFileConverter.Mappers;
using CommonFileConverter.Models;

namespace CommonFileConverter.Interfaces
{
    /// <summary>
    /// Defines implementation of class <typeparamref name="TFrom"/> that should provide ability to convert themself to instance of <typeparamref name="TTo"/> 
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <seealso cref="IInitializable<TFrom>"/>
    public interface IConvertible<TFrom> 
        where TFrom : BaseModel
    {
        /// <summary>
        /// Converts instance of type <typeparamref name="TFrom"/> to instance of <typeparamref name="TTo"/> 
        /// </summary>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="mapper">Instance of mapper that maps member of <typeparamref name="TFrom"/> to memeber of <typeparamref name="TTo"/> </param>
        /// <returns></returns>
        TTo Convert<TTo>(Mapper<TFrom, TTo> mapper) where TTo : IInitializable<TFrom>, new();
    }
}
