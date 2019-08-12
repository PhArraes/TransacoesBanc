using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Interfaces.Entities
{
    public interface IConta : IEntity
    {

        long Numero { get; }
        decimal Saldo { get; }

        IReadOnlyList<ILancamento> Lancamentos { get; }

        void AdicionarLancamento(ILancamento lancamento);
    }
}
