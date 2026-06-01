using Ferreteri.Models;

namespace Ferreteri.Services
{
    public interface ICategoriasService
    {
        Task<int> GetCount();
        Task<List<Categoria>> ListarCategorias();
        Task<int>AddCategoria(Categoria categoria);
        Task<int>RemoveCategoria(int id);



    }
}
