using Microsoft.Extensions.Options;
using PYPA.Transacoes.DataMapping.Database;
using PYPA.Transacoes.DataMapping.Interfaces;
using PYPA.Transacoes.DataMapping.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Factories
{
    public class ContaRepositoryFactory
    {
        DbConfiguration connectionStringProvider;

        public ContaRepositoryFactory(IOptions<DbConfiguration> dbConfiguration)
        {
            this.connectionStringProvider = dbConfiguration.Value;
        }

        public IContaRepository Create(SQLiteTransaction dBTransaction = null)
        {
            var cs = connectionStringProvider.ConnectionString;
            return dBTransaction == null ?
                new ContaRepository(cs) : new ContaRepository(cs, dBTransaction);
        }
    }
}
