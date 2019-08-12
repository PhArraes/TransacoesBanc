using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Interfaces.Entities
{
    public interface IEntity
    {
        Guid Id { get; }
        DateTime CreatedAt { get; }
    }
}
