using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Entities;

namespace PYPA.Transacoes.DataMapping.Database
{
    class UsuariosInit : IInitializer
    {
        public void Init(SQLiteConnection dBConnection)
        {
            CreateTable(dBConnection);
        }

        private void CreateTable(SQLiteConnection dBConnection)
        {
            string sql = @"create table IF NOT EXISTS Usuarios (
                                                    Id varchar(20), 
                                                    CreatedAt DATETIME,
                                                    PRIMARY KEY (Id))";

            SQLiteCommand command = new SQLiteCommand(sql, dBConnection);
            command.ExecuteNonQuery();

            Seed(dBConnection);
        }

        private void Seed(SQLiteConnection dBConnection)
        {
            var count = dBConnection.ExecuteScalar<int>(@"SELECT COUNT(Id)
                                                          FROM Usuarios;");
            if (count > 0) return;

            DateTimeProvider dateTimeProvider = new DateTimeProvider();
            dBConnection.Execute(@"
            INSERT INTO Usuarios (Id, CreatedAt)
            VALUES (@Id, @CreatedAt)",
                new Usuario(dateTimeProvider));
        }
    }
}
