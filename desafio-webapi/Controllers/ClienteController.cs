using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using desafio_core.Interface;
using desafio_core.Model;
using desafio_core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace desafio_webapi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteBusiness _business;
        private readonly IAuthBusiness _authbusiness;

        public ClienteController(IClienteBusiness business, IAuthBusiness authbusiness)
        {
            _business = business;
            _authbusiness = authbusiness;
        }

        [HttpPost]
        [Route("criar")]
        public async Task<ActionResult<bool>> Criar(Cliente entity)
        {
            try
            {
                return await _business.Criar(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize("Bearer")]
        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<List<Cliente>>> Get()
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.FirstOrDefault().Value;
                return await _business.GetAll();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("inserirusuario")]
        public async Task<ActionResult<bool>> inserirusuario(UsuarioViewModel vm)
        {
            try
            {
                return await _authbusiness.Criar(vm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UsuarioViewModel>> Login(UsuarioViewModel vm)
        {
            try
            {
                var result = await _authbusiness.Login(vm);
                if (result == null)
                {
                    return BadRequest("Não autorizado");
                }
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
