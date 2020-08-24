using desafio_core.Model;
using desafio_core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace desafio_core.Interface
{
    public interface IDividaBusiness
    {
        public Task<List<DividaViewModel>> BuscarPorCliente();
    }
}
