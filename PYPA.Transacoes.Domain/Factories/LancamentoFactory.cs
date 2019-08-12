using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Entities;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Factories
{
    public class LancamentoFactory
    {
        public static ILancamento Create(IConta conta, TipoDeLancamento tipo, decimal valor)
        {
            var dateProv = new DateTimeProvider();
            return new Lancamento(conta, tipo, valor, DateTime.Now, dateProv);
        }
    }
}
