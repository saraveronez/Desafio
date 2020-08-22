using System;
using System.Collections.Generic;
using System.Text;

namespace desafio_core.Model
{
    public class Cliente : BaseModel
    {
        public string Nome { get; set; }
        public string Identidade { get; set; }

        public virtual ICollection<Divida> Dividas { get; set; }
    }
}
