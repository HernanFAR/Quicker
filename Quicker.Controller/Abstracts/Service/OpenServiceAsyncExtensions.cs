using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;

namespace Quicker.Abstracts.Service
{
    public static partial class OpenServiceAsyncExtensions
    {
        public static Dictionary<string, string> GetPropertyInformationForCreate<TKey, TEntity>(
            this OpenServiceAsync<TKey, TEntity> service
        )
            where TEntity : class, IAbstractModel<TKey>
        {
            Dictionary<string, string> propertyTypes = new Dictionary<string, string>();

            Type entityType = typeof(TEntity);
            var propertyInfos = entityType.GetProperties();

            foreach (var propertyInfo in propertyInfos)
                propertyTypes.Add(propertyInfo.Name, propertyInfo.PropertyType.Name);

            return propertyTypes;
        }

        public static Dictionary<string, string> GetPropertyInformationForCreate<TKey, TEntity, TEntityDTO>(
            this OpenServiceAsync<TKey, TEntity, TEntityDTO> service
        )
            where TEntity : class, IAbstractModel<TKey>, IDomainOf<TEntityDTO>
            where TEntityDTO : class, IAbstractModel<TKey>, IDTOOf<TEntity>
        {
            Dictionary<string, string> propertyTypes = new Dictionary<string, string>();

            Type entityType = typeof(TEntityDTO);
            var propertyInfos = entityType.GetProperties();

            foreach (var propertyInfo in propertyInfos)
                propertyTypes.Add(propertyInfo.Name, propertyInfo.PropertyType.Name);

            return propertyTypes;
        }
    }
}
