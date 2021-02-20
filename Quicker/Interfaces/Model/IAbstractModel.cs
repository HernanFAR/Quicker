using System;

namespace Quicker.Interfaces.Model
{
    /// <summary>
    ///     Interface base para los campos minimos necesarios de las clases que seran consumidas por los servicios.
    /// </summary>
    /// <typeparam name="TKey">Representa el tipo de la PK en la base de datos. Suele ser <see cref="int"/> o <see cref="string"/></typeparam>
    /// 
    public interface IAbstractModel<TKey>
    {
        TKey Id { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime LastUpdated { get; set; }
    }
}
