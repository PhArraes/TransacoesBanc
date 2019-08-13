using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Interfaces
{
    public interface IContaRepository : IBaseRepository<IConta>
    {
        IConta Get(long numeroDaConta);
    }
}
