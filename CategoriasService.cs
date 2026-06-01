using Ferreteri.Models;
using Microsoft.EntityFrameworkCore;

namespace Ferreteri.Services
{
    public class CategoriasService : ICategoriasService
    {
        private readonly FerreteriContext _context;
        private readonly DbSet<Categoria> _dbSet;

        public CategoriasService(FerreteriContext context)
        {
            _context = context;
            _dbSet = _context.Set<Categoria>();
        }
        public async Task<int> AddCategoria(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
            int filasAfectadas = await _context.SaveChangesAsync();
            return filasAfectadas;
        }

        public Task<int> GetCount()
        {
            return _dbSet.CountAsync();
        }

        public async Task<List<Categoria>> ListarCategorias()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<int> RemoveCategoria(int id)
        {
            // Check if any Producto references this category
            bool tieneProductos = await _context.Productos
                .AnyAsync(p => p.FkIdCategoria == id);

            if (tieneProductos)
            {
                throw new InvalidOperationException("No se puede eliminar la categoría porque tiene productos asociados.");
            }

            return await _dbSet.Where(c => c.IdCategoria == id).ExecuteDeleteAsync();
        }

    }
}
