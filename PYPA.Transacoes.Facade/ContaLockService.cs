using PYPA.Transacoes.DataMapping;
using PYPA.Transacoes.DataMapping.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Facade
{
    public class ContaLockService
    {
        ContaLockRepositoryFactory contaLockRepositoryFactory;

        public ContaLockService(ContaLockRepositoryFactory contaLockRepositoryFactory)
        {
            this.contaLockRepositoryFactory = contaLockRepositoryFactory;
        }

        public bool PegarLock(long conta)
        {
            var contaLockRepository = contaLockRepositoryFactory.Create();
            return contaLockRepository.GetAndSetLock(conta);             
        }
        public bool PegarLocks(List<long> contas)
        {
            var contaLockRepository = contaLockRepositoryFactory.Create();
            var locksAdquiridos = new List<long>();
            foreach (var c in contas)
            {
                if (contaLockRepository.GetAndSetLock(c))
                    locksAdquiridos.Add(c);
                else {
                    LiberarLocks(locksAdquiridos);
                    return false;
                }
            }
            return locksAdquiridos.Count == contas.Count;
        }

        public bool LiberarLocks(List<long> contas)
        {
            var contaLockRepository = contaLockRepositoryFactory.Create();
            var released = true;
            foreach (var c in contas)
            {
                released = released && contaLockRepository.ReleaseLock(c);
            }
            return released;
        }
        public bool LiberarLock(long conta)
        {
            var contaLockRepository = contaLockRepositoryFactory.Create();
            return contaLockRepository.ReleaseLock(conta);
        }

    }
}
