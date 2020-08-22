using desafio_core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace desafio_core.Interface
{
    public interface IClienteBusiness
    {
        public Task<bool> Criar(Cliente entity);
        public Task<List<Cliente>> GetAll();
    }
}
