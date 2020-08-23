using desafio_core;
using desafio_core.Configuracoes;
using desafio_core.Interface;
using desafio_core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace desafio_core.Business
{
    public class AuthBusiness : IAuthBusiness
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        #region[Construtor]

        public AuthBusiness(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        #endregion

        #region [Métodos]

        public async Task<bool> Criar(UsuarioViewModel userInfo)
        {
            try
            {
                var identity = new IdentityUser
                {
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(identity);
                var user = await _userManager.FindByEmailAsync(identity.Email);
                var result = await _userManager.AddPasswordAsync(user, userInfo.Senha);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UsuarioViewModel> Login(UsuarioViewModel userInfo)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Senha, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return BuildToken(userInfo);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task LogOffAsync()
        {
            await _signInManager.SignOutAsync();
        }

        private UsuarioViewModel BuildToken(UsuarioViewModel userInfo)
        {
            var appSettingsSection = _configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Segredo);

            var tokenHandle = new JwtSecurityTokenHandler();

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject =  new ClaimsIdentity(new Claim[]
                {
                    new Claim("Usuario", userInfo.Email)
                }),
                Audience = appSettings.Validado,
                Expires = DateTime.UtcNow.AddHours(appSettings.ExpiracaoEmHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return new UsuarioViewModel
            {
                Email = userInfo.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(tokenHandle.CreateToken(tokenDescription))
            };
        }

        #endregion
    }
}
