using System;
using System.Collections.Generic;

namespace Ferreteri.Models;

public partial class Producto
{
    public int IdProd { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public short? Stock { get; set; }

    public int? FkIdCategoria { get; set; }

    public virtual Categoria? FkIdCategoriaNavigation { get; set; }

    public virtual ICollection<Movimiento>? Movimientos { get; set; } = new List<Movimiento>();
}
