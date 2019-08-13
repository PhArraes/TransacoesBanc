using Dapper;
using Microsoft.Extensions.Options;
using PYPA.Transacoes.DataMapping.Database.Dapper;
using PYPA.Transacoes.DataMapping.Database.Tables;
using PYPA.Transacoes.DataMapping.Interfaces;
using System;
using System.Data.SQLite;
using System.IO;

namespace PYPA.Transacoes.DataMapping.Database
{
    public class DatabaseInit
    {
        DbConfiguration connectionStringProvider;
        public DatabaseInit(IOptions<DbConfiguration> dbConfiguration)
        {
            this.connectionStringProvider = dbConfiguration.Value;
            file = connectionStringProvider.File;
        }
        private string file;
        public void Init()
        {
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            CreateDataBase();
        }

        private void CreateDataBase()
        {
            if (!File.Exists(file))
            {
                SQLiteConnection.CreateFile(file);
            }
            CreateTables();
        }

        private void CreateTables()
        {
            SQLiteConnection dbConnection = new SQLiteConnection(connectionStringProvider.ConnectionString);
            dbConnection.Open();
            new UsuariosInit().Init(dbConnection);
            new ContaInit().Init(dbConnection);
            new LancamentosInit().Init(dbConnection);
            new TransacoesInit().Init(dbConnection);
            dbConnection.Close();
        }
    }
}
