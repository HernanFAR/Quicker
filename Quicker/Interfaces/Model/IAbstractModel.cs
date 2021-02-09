using System;

namespace Quicker.Interfaces.Model
{

    /// <summary>.
    /// <para>Interface to base fields of all models that will use the services.</para>
    /// <para>It gifs: </para>
    /// <list type="bullet">
    /// <item>Id: <description>Identifier of the instance</description></item>
    /// <item>CreatedAt: <description>Datetime of when the instance was created</description></item>
    /// <item>LastUpdated: <description>Datetime of last update of instance</description></item>
    /// </list>
    /// </summary> 
    public interface IAbstractModel<TKey>
    {
        TKey Id { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime LastUpdated { get; set; }
    }
}
