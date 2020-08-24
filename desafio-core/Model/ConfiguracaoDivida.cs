using System;
using System.Collections.Generic;
using System.Text;

namespace desafio_core.Model
{
    public class ConfiguracaoDivida : BaseModel
    {

        public int QuantidadeMaximaParcelas { get; set; }
        public bool TipoJurosComposto { get; set; } //se true realizar calculo de juros composto
        public decimal Juros { get; set; }
        public decimal PorcentagemPaschoalotto { get; set; }

        public virtual ICollection<Divida> Dividas { get; set; }
    }
}
