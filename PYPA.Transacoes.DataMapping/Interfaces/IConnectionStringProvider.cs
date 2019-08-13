using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Interfaces
{
    public interface IConnectionStringProvider
    {
        String ConnectionString { get; }
        String File { get; }
    }
}
