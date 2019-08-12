using System;
using System.Collections.Generic;
using PYPA.Transacoes.Domain.Interfaces.Entities;

namespace PYPA.Transacoes.Domain.Interfaces.Entities
{
    public interface ITransacao : IEntity
    {
        IConta ContaOrigem { get; }
        Guid ContaOrigemId { get; }
        List<IConta> ContasDestino { get; }
        List<Guid> ContasDestinoIds { get; }
        decimal Valor { get; set; }
    }
}