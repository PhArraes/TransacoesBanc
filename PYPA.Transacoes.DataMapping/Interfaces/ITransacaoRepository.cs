using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Interfaces
{
    public interface ITransacaoRepository : IBaseRepository<ITransacao>
    {
        IEnumerable<ILancamento> Lancamentos(Guid transacaoId);
        void Save(ITransacao transacao);
    }
}
