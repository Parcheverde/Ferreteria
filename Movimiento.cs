using System;
using System.Collections.Generic;

namespace Ferreteri.Models;

public partial class Movimiento
{
    public int IdMov { get; set; }

    public int FkIdProd { get; set; }

    public string TipoMov { get; set; } = null!;

    public int Cantidad { get; set; }

    public DateOnly Fecha { get; set; }

    public virtual Producto? FkIdProdNavigation { get; set; }
}
