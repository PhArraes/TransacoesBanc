using Dapper;
using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Database.Tables
{
    class ContaInit : IInitializer
    {
        public void Init(SQLiteConnection dBConnection)
        {
            string sql = @"create table IF NOT EXISTS Contas (
                                                    Id varchar(20) NOT NULL, 
                                                    CreatedAt DATETIME NOT NULL,
                                                    Lock bit NOT NULL DEFAULT 0,
                                                    PRIMARY KEY (Id))";

            SQLiteCommand command = new SQLiteCommand(sql, dBConnection);
            command.ExecuteNonQuery();
            Seed(dBConnection);
        }
        private void Seed(SQLiteConnection dBConnection)
        {
            var count = dBConnection.ExecuteScalar<int>(@"SELECT COUNT(Id)
                                                          FROM Contas;");
            if (count > 0) return;

            var sql = @"
            INSERT INTO Contas (Id, CreatedAt)
            VALUES (@Id, @CreatedAt)";
            DateTimeProvider dateTimeProvider = new DateTimeProvider();
            dBConnection.Execute(sql, new Conta(dateTimeProvider));
            dBConnection.Execute(sql, new Conta(dateTimeProvider));
            dBConnection.Execute(sql, new Conta(dateTimeProvider));
            dBConnection.Execute(sql, new Conta(dateTimeProvider));
            dBConnection.Execute(sql, new Conta(dateTimeProvider));
            dBConnection.Execute(sql, new Conta(dateTimeProvider));
        }
    }
}
