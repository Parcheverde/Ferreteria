using Ferreteri.Models;

namespace Ferreteri.Services
{
    public interface IMovimientosService
    {
        Task<int> GetCount();
        Task<List<Movimiento>> GetAll();
        Task<List<Movimiento>> GetByTipo(string tipo);
        Task<int> AddMovimiento(Movimiento movimiento);
    }
}
