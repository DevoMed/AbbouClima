using AbbouClima.Models;

namespace AbbouClima.Interfaces
{
    public interface IPresupuestoService
    {
        Task<List<Presupuesto>> GetAll(int? id, string busqueda);
        Task<Presupuesto> CdetailsById(int id);
        Task<Presupuesto> PdetailsById(Guid id);
        Task CreatePresupuesto(Presupuesto presupuesto);
        Task EditPresupuesto(Guid id, Presupuesto presupuesto);

    }
}
