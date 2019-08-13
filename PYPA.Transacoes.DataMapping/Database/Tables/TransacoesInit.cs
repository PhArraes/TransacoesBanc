using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Entities;

namespace PYPA.Transacoes.DataMapping.Database
{
    class TransacoesInit : IInitializer
    {
        public void Init(SQLiteConnection dBConnection)
        {
            CreateTable(dBConnection);
            CreateRelTable(dBConnection);
        }

        private void CreateTable(SQLiteConnection dBConnection)
        {
            string sql = @"create table IF NOT EXISTS Transacoes (
                                                    Id varchar(20), 
                                                    CreatedAt DATETIME NOT NULL,
                                                    ContaOrigemId varchar(20) NOT NULL,
                                                    UsuarioResponsavelId varchar(20) NOT NULL,
                                                    Valor real NOT NULL,
                                                    PRIMARY KEY (Id),
                                                    FOREIGN KEY (ContaOrigemId) REFERENCES Contas (Id))";

            SQLiteCommand command = new SQLiteCommand(sql, dBConnection);
            command.ExecuteNonQuery();
        }

        private void CreateRelTable(SQLiteConnection dBConnection)
        {
            string sql = @"create table IF NOT EXISTS Transacoes_Lancamentos (
                                                    TransacaoId varchar(20) NOT NULL, 
                                                    LancamentoId varchar(20) NOT NULL,
                                                    PRIMARY KEY (TransacaoId, LancamentoId),
                                                    FOREIGN KEY (TransacaoId) REFERENCES Transacoes (Id),
                                                    FOREIGN KEY (LancamentoId) REFERENCES Lancamentos (Id))";

            SQLiteCommand command = new SQLiteCommand(sql, dBConnection);
            command.ExecuteNonQuery();
        }
    }
}
