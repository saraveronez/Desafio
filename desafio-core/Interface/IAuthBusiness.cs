using System.Threading.Tasks;
using desafio_core.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace desafio_core.Interface
{
    public interface IAuthBusiness
    {
        public Task<UsuarioViewModel> Login(UsuarioViewModel userInfo);
        public Task LogOffAsync();
        public Task<bool> Criar(UsuarioViewModel userInfo);
    }
}
