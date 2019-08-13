using PYPA.Transacoes.DataMapping.Interfaces;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;

namespace PYPA.Transacoes.DataMapping
{
    public interface IContaLockRepository : IBaseRepository<IContaLock>
    {
        bool GetAndSetLock(long numeroConta);
        bool GetAndSetLock(Guid id);
        bool ReleaseLock(long numeroConta);
        bool ReleaseLock(Guid id);
    }
}