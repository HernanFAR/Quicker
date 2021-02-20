namespace Quicker.Interfaces.Model
{

    /// <summary>
    ///     <em>Marker Interface</em> para señalar que esta clase es la entidad DTO del dominio relacionado.
    /// </summary>
    /// <typeparam name="TDomain">Entidad Dominio que se relaciona con esta entidad de DTO. Debe ser una clase</typeparam>
    /// 
    public interface IDTOOf<TDomain>
        where TDomain : class
    {
    }
}
