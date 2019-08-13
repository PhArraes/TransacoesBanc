using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T Get(Guid id);
        IEnumerable<T> ListAll();
    }
}
