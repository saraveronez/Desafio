using desafio_core.Interface;
using desafio_core.Model;
using desafio_core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desafio_core.Business
{
    public class ClienteBusiness : IClienteBusiness
    {
        private readonly ApplicationDbContext _context;

        public ClienteBusiness(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Criar(ClienteViewModel vm)
        {
            try
            {
                await _context.Cliente.AddAsync(new Cliente(vm.Nome, vm.Identidade));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ClienteViewModel>> GetAll()
        {
            try
            {
                return await _context.Cliente.AsNoTracking().Select(x => new ClienteViewModel
                {
                    Identidade = x.Identidade,
                    Nome = x.Nome
                }).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }


        
    }
}
