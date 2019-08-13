using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Entities
{
    public class ContaLock : IContaLock
    {
        public Guid Id { get; private set; }
        public bool Lock { get; private set; }
        private ContaLock() { }
    }
}
