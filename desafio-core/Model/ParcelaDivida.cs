using System;
using System.Collections.Generic;
using System.Text;

namespace desafio_core.Model
{
    public class ParcelaDivida : BaseModel
    {
        public Guid DividaId { get; set; }
        public DateTime DataVencimento { get; set; }
        public int QuantidadeParcelas { get; set; }
        public decimal ValorOriginal { get; set; }
        public int DiasAtraso { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal ValorFinal { get; set; }


        public virtual Divida Divida { get; set; }
    }
}
