using Microsoft.Extensions.Options;
using PYPA.Transacoes.DataMapping.Database;
using PYPA.Transacoes.DataMapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Factories
{
    public class TransactionFactory
    {
        DbConfiguration connectionStringProvider;

        public TransactionFactory(IOptions<DbConfiguration> dbConfiguration)
        {
            this.connectionStringProvider = dbConfiguration.Value;
        }

        public SQLiteTransaction CreateSQLTransaction()
        {
            SQLiteConnection _connection = new SQLiteConnection(connectionStringProvider.ConnectionString);
            _connection.Open();
            SQLiteTransaction _transaction = _connection.BeginTransaction();

            return _transaction;
        }
    }
}
