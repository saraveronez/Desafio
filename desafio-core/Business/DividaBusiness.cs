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
    public class DividaBusiness : IDividaBusiness
    {
        private readonly ApplicationDbContext _context;

        public DividaBusiness(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DividaViewModel>> BuscarPorCliente(string userId)
        {
            try
            {
                return await _context.Divida.Include(d => d.ConfiguracaoDivida)
                .Include(d => d.Cliente.IdentityUser)
                .Include(d => d.Parcelas)
                .AsNoTracking().Select(x => new DividaViewModel
                {
                    NumeroParcelas = x.Parcelas.Count,
                    ValorTotal = x.Valor,
                    UserId = x.Cliente.IdentityUser.Id,
                    QuantidadeMaximaParcelas = x.ConfiguracaoDivida.QuantidadeMaximaParcelas,
                    DataVencimento = x.DataVencimento
                }).Where(d => d.UserId == userId).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async void CalcularParcelas(Guid dividaId)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    var divida = _context.Divida.Include(d => d.ConfiguracaoDivida).Where(d => d.Id == dividaId).FirstOrDefault();
                    if (divida.Parcelas.Any())
                        return;
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
                            var pagamento = Math.Round(Convert.ToDecimal((valor * Math.Pow((taxaJuros / 12) + 1, (parcelas)) * taxaJuros / 12)
                                                / (Math.Pow(taxaJuros / 12 + 1, (parcelas)) - 1)), 2);
                            divida.ValorFinalComJuros = Math.Round(pagamento * parcelas, 2);
                            divida.ValorJuros = Math.Round(divida.ValorFinalComJuros - divida.Valor, 2);
                        }
                        else
                        {
                            divida.DiasAtraso = (int)(DateTime.UtcNow - divida.DataVencimento).TotalDays;
                            divida.ValorFinalComJuros = divida.Valor * (1 + (divida.ConfiguracaoDivida.Juros + divida.DiasAtraso));
                            divida.ValorJuros = divida.ValorFinalComJuros - divida.Valor;
                            divida.ValorComissaoPaschoalotto = (divida.ValorFinalComJuros * divida.ConfiguracaoDivida.PorcentagemPaschoalotto) / 100;
                        }

                        for (var i = 1; i <= parcelas; i++)
                        {
                            var parcelaDivida = new ParcelaDivida
                            {
                                DataVencimento = DateTime.UtcNow.AddDays(i),
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
                    _context.SaveChanges();
                    await trans.CommitAsync();
                }

            }
            catch (Exception) { }
        }


    }
}
