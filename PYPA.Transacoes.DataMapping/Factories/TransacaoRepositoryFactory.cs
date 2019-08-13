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
    public class TransacaoRepositoryFactory
    {
        DbConfiguration connectionStringProvider;

        public TransacaoRepositoryFactory(IOptions<DbConfiguration> dbConfiguration)
        {
            this.connectionStringProvider = dbConfiguration.Value;
        }

        public ITransacaoRepository Create(SQLiteTransaction dBTransaction = null)
        {
            var cs = connectionStringProvider.ConnectionString;
            return dBTransaction == null ?
                new TransacaoRepository(cs) : new TransacaoRepository(cs, dBTransaction);
        }
    }
}
