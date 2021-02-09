using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quicker.Abstracts.Model
{
    /// <summary>.
    /// <para>Main implementation of <seealso cref="IAbstractModel{TKey}"/>.</para>
    /// </summary> 
    public abstract class AbstractModel<TKey> : IAbstractModel<TKey>
    {
        /// <summary>.
        /// <para>Implementation of <seealso cref="IAbstractModel{TKey}.Id"/>.</para>
        /// <para>Is Key and Required.</para>
        /// </summary> 
        [Key, Required]
        public TKey Id { get; set; }

        /// <summary>.
        /// <para>Implementation of <seealso cref="IAbstractModel{TKey}.CreatedAt"/>.</para>
        /// <para>Is Required, and it default value is the <em>current date</em>.</para>
        /// </summary> 
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>.
        /// <para>Implementation of <seealso cref="IAbstractModel{TKey}.LastUpdated"/>.</para>
        /// <para>Is Required, and it default value is the <em>current date</em>.</para>
        /// </summary> 
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
