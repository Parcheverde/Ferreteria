using Ferreteri.Models;

namespace Ferreteri.Services
{
    public interface IProductosService
    {
        Task<int> GetCount();
        Task<List<Producto>> ListarProductos();
        Task<int> AddProducto(Producto producto);
        Task<int> RemoveProducto(int id);

        Task<int> UpdateProducto(Producto producto);
    }
}
