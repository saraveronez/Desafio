using desafio_core.Interface;
using desafio_core.Model;
using desafio_core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desafio_core.Business
{
    public class DividaBusiness : IDividaBusiness
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;



        public DividaBusiness(ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public async Task<List<DividaViewModel>> BuscarPorCliente()
        {
            try
            {
                var userId = _httpContext.HttpContext.User.Claims.First(c => c.Type == "userId");
                await this.CalcularParcelas(userId.Value);
                return await _context.Divida.Include(d => d.ConfiguracaoDivida)
                .Include(d => d.Cliente.IdentityUser)
                .Include(d => d.Parcelas)
                .AsNoTracking().Select(x => new DividaViewModel
                {
                    NumeroParcelas = x.Parcelas.Count,
                    ValorTotal = x.Valor,
                    ValorComJuros = x.ValorFinalComJuros,
                    UserId = x.Cliente.IdentityUser.Id,
                    QuantidadeMaximaParcelas = x.ConfiguracaoDivida.QuantidadeMaximaParcelas,
                    DataVencimento = x.DataVencimento,
                    Parcelas = x.Parcelas.Select(p => new DividaParcelaViewModel
                    {
                        DataVencimento = p.DataVencimento,
                        NumeroParcela = p.NumeroParcela,
                        Pago = p.Pago,
                        ValorComJuros = p.ValorComJuros,
                        ValorOriginal = p.ValorOriginal
                    }).ToList()
                }).Where(d => d.UserId == userId.Value).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<bool> CalcularParcelas(string userId)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    var lista = await _context.Divida.Include(d => d.ConfiguracaoDivida)
                    .Include(d => d.Cliente.IdentityUser)
                    .Include(d => d.Parcelas).Where(d => d.Cliente.IdentityUser.Id == userId).ToListAsync();

                    foreach (var divida in lista)
                    {
                        if (divida.Parcelas.Any())
                            continue;
                        var parcelas = divida.ConfiguracaoDivida.QuantidadeMaximaParcelas;

                        if (divida != null && divida.DataVencimento < DateTime.UtcNow)
                        {
                            if (divida.ConfiguracaoDivida.TipoJurosComposto)
                            {
                                var taxaJuros = Convert.ToDouble(divida.ConfiguracaoDivida.Juros);
                                var valor = Convert.ToDouble(divida.Valor);
                                if (taxaJuros >= 1)
                                {
                                    taxaJuros = taxaJuros / 100;
                                }
                                var jurosporparcela = Math.Round(Convert.ToDecimal((valor * Math.Pow((taxaJuros / 12) + 1, (parcelas)) * taxaJuros / 12)
                                                    / (Math.Pow(taxaJuros / 12 + 1, (parcelas)) - 1)), 2);
                                divida.ValorFinalComJuros = divida.Valor + jurosporparcela;
                                divida.ValorJuros = Math.Round(divida.ValorFinalComJuros - divida.Valor, 2);
                            }
                            else
                            {
                                divida.DiasAtraso = (int)(DateTime.UtcNow - divida.DataVencimento).TotalDays;
                                divida.ValorFinalComJuros = divida.Valor * (1 + ((divida.ConfiguracaoDivida.Juros /100) * divida.DiasAtraso));
                                divida.ValorJuros = divida.ValorFinalComJuros - divida.Valor;
                            }
                            divida.ValorComissaoPaschoalotto = (divida.ValorFinalComJuros * divida.ConfiguracaoDivida.PorcentagemPaschoalotto) / 100;
                            for (var i = 1; i <= parcelas; i++)
                            {
                                var parcelaDivida = new ParcelaDivida
                                {
                                    DataVencimento = i == 1 ? DateTime.UtcNow.AddDays(i) : DateTime.UtcNow.AddDays(1).AddMonths(i - 1),
                                    DividaId = divida.Id,
                                    Divida = divida,
                                    NumeroParcela = i,
                                    Pago = false,
                                    ValorOriginal = divida.Valor / parcelas,
                                    ValorComJuros = divida.ValorFinalComJuros / parcelas
                                };

                                divida.Parcelas.Add(parcelaDivida);
                            }
                            _context.Divida.Update(divida);
                        }
                    }

                    _context.SaveChanges();
                    await trans.CommitAsync();
                }
                return true;
            }
            catch (Exception) { return false; }
        }


    }
}
