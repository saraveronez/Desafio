using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using desafio_core.Interface;
using desafio_core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace desafio_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteBusiness _business;

        public ClienteController(IClienteBusiness business)
        {
            _business = business;
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

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<List<Cliente>>> Get()
        {
            try
            {
                return await _business.GetAll();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
