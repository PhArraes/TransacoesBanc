using Microsoft.Extensions.Options;
using PYPA.Transacoes.DataMapping.Database;
using PYPA.Transacoes.DataMapping.Interfaces;
using System;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace PYPA.Transacoes.DataMapping.Factories
{
    public class ContaLockRepositoryFactory
    {
        DbConfiguration connectionStringProvider;

        public ContaLockRepositoryFactory(IOptions<DbConfiguration> dbConfiguration)
        {
            this.connectionStringProvider = dbConfiguration.Value;
        }

        public IContaLockRepository Create(SQLiteTransaction dBTransaction = null)
        {
            var cs = connectionStringProvider.ConnectionString;
            return dBTransaction == null ?
                new ContaLockRepository(cs) : new ContaLockRepository(cs, dBTransaction);
        }
    }
}
