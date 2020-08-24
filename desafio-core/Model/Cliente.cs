using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace desafio_core.Model
{
    public class Cliente : BaseModel
    {

        public Cliente() { }

        public Cliente(string nome, string identidade)
        {

            this.Nome = nome;
            this.Identidade = identidade;
        }
        public string Nome { get; set; }
        public string Identidade { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public virtual ICollection<Divida> Dividas { get; set; }
    }
}
