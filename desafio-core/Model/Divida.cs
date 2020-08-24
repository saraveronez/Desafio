using System;
using System.Collections.Generic;
using System.Text;

namespace desafio_core.Model
{
    public class Divida : BaseModel
    {

        public Guid ClienteId { get; set; }
        public Guid ConfiguracaoDividaId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public int DiasAtraso { get; set; }
        public decimal  ValorJuros { get; set; }
        public decimal  ValorFinalComJuros { get; set; }
        public decimal ValorComissaoPaschoalotto { get; set; }


        public virtual Cliente Cliente { get; set; }
        public virtual ConfiguracaoDivida ConfiguracaoDivida { get; set; }
        public virtual ICollection<ParcelaDivida> Parcelas { get; set; } = new HashSet<ParcelaDivida>();
    }
}
