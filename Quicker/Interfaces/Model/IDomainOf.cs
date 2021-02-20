namespace Quicker.Interfaces.Model
{
    /// <summary>
    ///     <em>Marker Interface</em> para señalar que esta clase es la entidad de dominio del DTO relacionado.
    /// </summary>
    /// <typeparam name="TDTO">Entidad DTO que se relaciona con esta entidad de Dominio. Debe ser una clase</typeparam>
    /// 
    public interface IDomainOf<TDTO>
        where TDTO : class
    {
    }
}
