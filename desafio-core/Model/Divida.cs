using System;
using System.Collections.Generic;
using System.Text;

namespace desafio_core.Model
{
    public class Divida : BaseModel
    {

        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }

        public virtual Cliente Cliente { get; set; } 
        public virtual ICollection<ParcelaDivida> Parcelas { get; set; }
    }
}
