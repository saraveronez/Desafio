using System;
using System.Collections.Generic;
using System.Text;

namespace desafio_core.ViewModels
{
    public class DividaViewModel
    {
        public Guid Id { get; set; }
        public decimal ValorTotal { get; set; }
        public string UserId { get; set; }
        public int NumeroParcelas { get; set; }
        public DateTime DataVencimento { get; set; }
        public int QuantidadeMaximaParcelas { get; set; }
        public decimal ValorComJuros { get; set; }
    }
}
