using Quicker.Interfaces.Model;
using System.Collections.Generic;

namespace Quicker.Interfaces.Service
{
    public static partial class OpenServiceAsyncExtensions
    {
        public static Dictionary<string, string> GetPropertyInformationForCreate<TKey, TEntity>(
            this IOpenServiceAsync<TKey, TEntity> service
        )
            where TEntity : class, IAbstractModel<TKey>
        {
            return new Dictionary<string, string>();
        }

        public static Dictionary<string, string> GetPropertyInformationForCreate<TKey, TEntity, TEntityDTO>(
            this IOpenServiceAsync<TKey, TEntity, TEntityDTO> service
        )
            where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
            where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
        {
            return new Dictionary<string, string>();
        }
    }
}
