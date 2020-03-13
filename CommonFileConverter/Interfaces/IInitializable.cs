using CommonFileConverter.Models;

namespace CommonFileConverter.Interfaces
{
    /// <summary>
    /// Defines implementation of class that should be initialized based on instance of <typeparamref name="TFrom"/> 
    /// </summary>
    /// <typeparam name="TFrom">class which members are used for initialization instance of class that implements <c>IInitializable<TFrom></c></typeparam>
    public interface IInitializable<TFrom> where TFrom : BaseModel
    {
        /// <summary>
        /// Initializes members of class that implements interface <c>IInitializable<TFrom></c> based on members of instance <typeparamref name="TFrom"/> 
        /// </summary>
        /// <param name="from"></param>
        void Initialize(TFrom from);
    }
}
