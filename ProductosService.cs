// Services/ProductosService.cs (updated)
using Ferreteri.Models;
using Microsoft.EntityFrameworkCore;

namespace Ferreteri.Services
{
    public class ProductosService : IProductosService
    {
        private readonly FerreteriContext _context;
        private readonly DbSet<Producto> _dbSet;
        private readonly DbSet<Movimiento> _movimientosDbSet;

        public ProductosService(FerreteriContext context)
        {
            _context = context;
            _dbSet = _context.Set<Producto>();
            _movimientosDbSet = _context.Set<Movimiento>();
        }

        public async Task<int> AddProducto(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("El nombre del producto no puede estar vacío.");

            if (producto.Precio <= 0)
                throw new ArgumentException("El precio del producto debe ser un valor mayor a cero.");

            if (producto.Stock < 0)
                throw new ArgumentException("El stock inicial del producto no puede ser un número negativo.");

            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.IdCategoria == producto.FkIdCategoria);
            if (!categoriaExiste)
                throw new KeyNotFoundException("La categoría asignada al producto no existe en la base de datos.");

            producto.FkIdCategoriaNavigation = null;
            producto.Movimientos = null;

            await _dbSet.AddAsync(producto);
            await _context.SaveChangesAsync();

            var movimiento = new Movimiento
            {
                FkIdProd = producto.IdProd,
                TipoMov = "CREACION".ToLowerInvariant(),
                Cantidad = producto.Stock ?? 0,
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                FkIdProdNavigation = null
            };

            await _movimientosDbSet.AddAsync(movimiento);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> GetCount()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<List<Producto>> ListarProductos()
        {
            return await _dbSet.Include(p => p.FkIdCategoriaNavigation).ToListAsync();
        }

        public async Task<int> RemoveProducto(int id)
        {
            var producto = await _dbSet.FirstOrDefaultAsync(p => p.IdProd == id);
            if (producto == null)
                return 0;

            // Validamos que no tenga transacciones de compras/ventas reales
            var tieneMovimientosReales = await _context.Movimientos.AnyAsync(m => m.FkIdProd == id && m.TipoMov != "creacion");
            if (tieneMovimientosReales)
            {
                throw new InvalidOperationException("No se puede eliminar el producto porque ya cuenta con transacciones de inventario operativas.");
            }

            var movimientosDeCreacion = await _context.Movimientos.Where(m => m.FkIdProd == id && m.TipoMov == "creacion").ToListAsync();
            if (movimientosDeCreacion.Any())
            {
                _context.Movimientos.RemoveRange(movimientosDeCreacion);
            }

            // Desvinculamos las listas virtuales de C# por seguridad
            producto.Movimientos = null;
            producto.FkIdCategoriaNavigation = null;

            //Eliminamos el producto de forma limpia
            _dbSet.Remove(producto);

            //Guardamos los cambios de una sola vez
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateProducto(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("El nombre del producto no puede quedar vacío.");

            if (producto.Precio <= 0)
                throw new ArgumentException("El precio modificado debe ser un valor mayor a cero.");

            if (producto.Stock < 0)
                throw new ArgumentException("El stock no puede modificarse a un valor negativo.");

            var productoActual = await _dbSet.FirstOrDefaultAsync(p => p.IdProd == producto.IdProd);
            if (productoActual == null)
                return 0;

            int stockAnterior = productoActual.Stock ?? 0;

            productoActual.Nombre = producto.Nombre;
            productoActual.Precio = producto.Precio;
            productoActual.Stock = producto.Stock;
            productoActual.FkIdCategoria = producto.FkIdCategoria;

            if (stockAnterior != (producto.Stock ?? 0))
            {
                var movimiento = new Movimiento
                {
                    FkIdProd = producto.IdProd,
                    TipoMov = "AJUSTE".ToLowerInvariant(),
                    Cantidad = (producto.Stock ?? 0) - stockAnterior,
                    Fecha = DateOnly.FromDateTime(DateTime.Now)
                };

                await _movimientosDbSet.AddAsync(movimiento);
            }

            return await _context.SaveChangesAsync();
        }
    }
}