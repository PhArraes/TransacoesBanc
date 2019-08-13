using Dapper;
using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Database.Tables
{
    class LancamentosInit : IInitializer
    {
        public void Init(SQLiteConnection dBConnection)
        {
            string sql = @"create table IF NOT EXISTS Lancamentos (
                                                    Id varchar(20) NOT NULL, 
                                                    CreatedAt DATETIME NOT NULL,
                                                    ContaId varchar(20) NOT NULL,
                                                    DataDoLancamento DATETIME,
                                                    Tipo INT(1),
                                                    Valor REAL,
                                                    PRIMARY KEY (Id),
                                                    FOREIGN KEY (ContaId) REFERENCES Contas (Id))";

            SQLiteCommand command = new SQLiteCommand(sql, dBConnection);
            command.ExecuteNonQuery();
            Seed(dBConnection);
        }
        private void Seed(SQLiteConnection db)
        {
            var count = db.ExecuteScalar<int>(@"SELECT COUNT(Id)
                                                          FROM Lancamentos;");
            if (count > 0) return;

            var sql = @"
            INSERT INTO Lancamentos (Id, CreatedAt, Valor, DataDoLancamento, ContaId, Tipo)
            VALUES (@Id, @CreatedAt, @Valor, @DataDoLancamento, @ContaId, @Tipo)";
            DateTimeProvider dateTimeProvider = new DateTimeProvider();

            var contas = db.Query<Conta>(@"SELECT Id, CreatedAt FROM Contas");
            contas.AsList().ForEach(c =>
            {
                db.Execute(sql, new Lancamento(c, Domain.TipoDeLancamento.Credito, 500, DateTime.Now, dateTimeProvider));
            });
        }
    }
}
