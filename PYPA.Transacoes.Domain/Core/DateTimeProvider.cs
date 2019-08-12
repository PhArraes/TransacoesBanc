using PYPA.Transacoes.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Core
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;

    }
}
