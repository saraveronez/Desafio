using desafio_core.Interface;
using desafio_core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<bool> Criar(Cliente entity)
        {
            try
            {
                await _context.Cliente.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Cliente>> GetAll()
        {
            try
            {
                return await _context.Cliente.ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
