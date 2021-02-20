using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quicker.Abstracts.Model
{
    /// <summary>
    ///     Implementacion principal de <see cref="IAbstractModel{TKey}"/>, que da valores predeterminados a los campos de dicha interface.
    /// </summary>
    /// <typeparam name="TKey">Representa el tipo de la PK en la base de datos. Suele ser <see cref="int"/> o <see cref="string"/></typeparam>
    /// 
    public abstract class AbstractModel<TKey> : IAbstractModel<TKey>
    {
        [Key, Required]
        public TKey Id { get; set; } = default;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
