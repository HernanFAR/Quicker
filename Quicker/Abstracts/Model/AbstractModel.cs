using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quicker.Abstracts.Model
{
    public abstract class AbstractModel<TKey> : IAbstractModel<TKey>
    {
        [Key, Required]
        public TKey Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
