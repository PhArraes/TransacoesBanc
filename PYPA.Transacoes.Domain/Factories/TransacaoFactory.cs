using PYPA.Transacoes.Domain.Entities;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Factories
{
    public class TransacaoFactory
    {
        IDateTimeProvider dateTimeProvider;

        public TransacaoFactory(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        public ITransacao Create(IUsuario usuario, IConta conta, List<IConta> contasDestino, decimal valor)
        {
            return new Transacao(usuario, conta, contasDestino, valor, dateTimeProvider);
        }
    }
}
