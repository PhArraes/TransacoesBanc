using Dapper;
using PYPA.Transacoes.DataMapping.Database.Dapper;
using PYPA.Transacoes.DataMapping.Database.Tables;
using System;
using System.Data.SQLite;
using System.IO;

namespace PYPA.Transacoes.DataMapping.Database
{
    public class DatabaseInit
    {
        private string file = "transacoes.sqlite";
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
            SQLiteConnection dbConnection = new SQLiteConnection($"Data Source={file};Version=3;");
            dbConnection.Open();
            new UsuariosInit().Init(dbConnection);
            new ContaInit().Init(dbConnection);
            new LancamentosInit().Init(dbConnection);
            dbConnection.Close();
        }
    }
}
