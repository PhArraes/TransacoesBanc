using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(String message) : base(message)
        {

        }
    }
}
