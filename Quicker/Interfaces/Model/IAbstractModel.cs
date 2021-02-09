using System;

namespace Quicker.Interfaces.Model
{
    public interface IAbstractModel<TKey>
    {
        TKey Id { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime LastUpdated { get; set; }
    }
}
