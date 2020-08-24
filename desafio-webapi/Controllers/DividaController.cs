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
    [Authorize("Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DividaController : ControllerBase
    {
        private readonly IDividaBusiness _business;

        public DividaController(IDividaBusiness business)
        {
            _business = business;
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<List<DividaViewModel>>> Get()
        {
            try
            {
                return await _business.BuscarPorCliente();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
