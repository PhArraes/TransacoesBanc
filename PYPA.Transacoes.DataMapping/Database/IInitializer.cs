using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Database
{
    interface IInitializer
    {
        void Init(SQLiteConnection dBConnection);
    }
}
