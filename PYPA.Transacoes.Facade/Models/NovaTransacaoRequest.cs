using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Facade.Models
{
    public class NovaTransacaoRequest
    {
        public Guid Usuario { get; set; }
        public long ContaOrigem { get; set; }
        public List<long> ContasDestino { get; set; }
        public decimal Valor { get; set; }

    }
}
