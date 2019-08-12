using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Interfaces.Core
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
