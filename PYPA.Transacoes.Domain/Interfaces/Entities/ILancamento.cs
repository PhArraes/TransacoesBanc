using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Interfaces.Entities
{
    public interface ILancamento : IEntity
    {
        TipoDeLancamento Tipo { get; }
        Guid ContaId { get; }
        Decimal Valor { get; }
        DateTime DataDoLancamento { get; }
        Guid TransacaoId { get; }
    }
}
