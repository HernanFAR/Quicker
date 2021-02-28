using Quicker.Interfaces.Model;
using System.Collections.Generic;

namespace Quicker.Interfaces.Service
{
    public static partial class FullServiceAsyncExtensions
    {
        public static Dictionary<string, string> GetPropertyInformationForUpdating<TKey, TEntity>(
            this IFullServiceAsync<TKey, TEntity> service
        )
            where TEntity : class, IAbstractModel<TKey>
        {
            return new Dictionary<string, string>();
        }

        public static Dictionary<string, string> GetPropertyInformationForUpdating<TKey, TEntity, TEntityDTO>(
            this IFullServiceAsync<TKey, TEntity, TEntityDTO> service
        )
            where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
            where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
        {
            return new Dictionary<string, string>();
        }
    }
}
