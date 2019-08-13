using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.DataMapping
{
    public class ContaLockRepository : IContaLockRepository
    {
        public bool GetAndSetLock(long numeroConta)
        {
            throw new NotImplementedException();
        }


        public bool ReleaseLock(long numeroConta)
        {
            throw new NotImplementedException();
        }
    }
}
