using Ferreteri.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferreteri.Services
{
    public class MovimientosService : IMovimientosService
    {
        private readonly FerreteriContext _context;
        private readonly DbSet<Movimiento> _dbSet;

        public MovimientosService(FerreteriContext context)
        {
            _context = context;
            _dbSet = _context.Set<Movimiento>();
        }

        public async Task<int> GetCount()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<List<Movimiento>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<Movimiento>> GetByTipo(string tipo)
        {
            return await _dbSet
                .Where(m => m.TipoMov.ToLower() == tipo.ToLower())
                .ToListAsync();
        }

        public async Task<int> AddMovimiento(Movimiento movimiento)
        {
            movimiento.TipoMov = movimiento.TipoMov.ToUpper().Trim();
            
            if (movimiento.TipoMov != "ENTRADA" && movimiento.TipoMov != "SALIDA")
            {
                throw new ArgumentException("El tipo de movimiento debe ser 'ENTRADA' o 'SALIDA'.");
            }

            var producto = await _context.Productos.FindAsync(movimiento.FkIdProd);
            if (producto == null) 
            { 
                throw new ArgumentException("El producto especificado no existe.");
            }

            if (movimiento.TipoMov == "ENTRADA")
            {
                producto.Stock = (short)((producto.Stock ?? 0) + movimiento.Cantidad);
            }
            else if (movimiento.TipoMov == "SALIDA")
            {
                //validamos si hay existencias
                if ((producto.Stock ?? 0) < movimiento.Cantidad)
                {
                    throw new InvalidOperationException("No hay suficiente stock para realizar la salida.");
                }
                producto.Stock = (short)((producto.Stock ?? 0) - movimiento.Cantidad);
            }

            movimiento.Fecha = DateOnly.FromDateTime(DateTime.Now);
            _dbSet.Add(movimiento);
            _context.Entry(producto).State = EntityState.Modified;

            return await _context.SaveChangesAsync();
        }
    }
}
