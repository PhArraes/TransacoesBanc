using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Interfaces.Entities
{
    public interface IContaLock
    {
        Guid Id { get; }
        bool Lock { get; }
    }
}
