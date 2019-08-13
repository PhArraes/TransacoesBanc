using PYPA.Transacoes.DataMapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Facade
{
    class ContaLockService
    {
        IContaLockRepository contaLockRepository;


        public void PegarLocks(List<long> contas)
        {
            var locksAdquiridos = new List<long>();
            contas.ForEach(c =>
            {
                if (contaLockRepository.GetAndSetLock(c))
                    locksAdquiridos.Add(c);
            });
        }

        public bool LiberarLocks(List<long> contas)
        {
            var released = false;
            //contas.ForEach(c => {
            //    if (!contaLockRepository.ReleaseLock(c))
            //    {
            //        released
            //    }
            //        });
            return released;
        }

    }
}
