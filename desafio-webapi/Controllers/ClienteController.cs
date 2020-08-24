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
        private readonly IDividaBusiness _dividaBusiness;

        public ClienteController(IClienteBusiness business, IAuthBusiness authbusiness, IDividaBusiness dividaBusiness)
        {
            _business = business;
            _authbusiness = authbusiness;
            _dividaBusiness = dividaBusiness;
        }

        [Authorize("Bearer")]
        [HttpPost]
        [Route("criar")]
        public async Task<ActionResult<bool>> Criar(ClienteViewModel vm)
        {
            try
            {
                return await _business.Criar(vm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize("Bearer")]
        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<List<ClienteViewModel>>> Get()
        {
            try
            {
                _dividaBusiness.CalcularParcelas(new Guid("e30e48ab-2fe4-469e-89eb-4fe45a679ae2"));
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
