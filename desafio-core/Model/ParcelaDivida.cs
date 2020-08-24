using System;
using System.Collections.Generic;
using System.Text;

namespace desafio_core.Model
{
    public class ParcelaDivida : BaseModel
    {
        public Guid DividaId { get; set; }
        public DateTime DataVencimento { get; set; }
        public int NumeroParcela { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorComJuros { get; set; }
        public bool Pago { get; set; }

        public virtual Divida Divida { get; set; }
    }
}
