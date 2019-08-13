using PYPA.Transacoes.DataMapping.Interfaces;
using PYPA.Transacoes.Domain.Entities;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Repositories
{
    class ContaRepository : BaseRepository<Conta, IConta>, IContaRepository
    {
        public ContaRepository(string connectionString) : base(connectionString)
        {
        }

        public ContaRepository(string connectionString, SQLiteTransaction sqlTransaction) : base(connectionString, sqlTransaction)
        {
        }

        public override IConta Get(Guid Id)
        {
            var sql = @"SELECT distinct c.Id,
                               c.CreatedAt,
                               SUM(CASE l.Tipo WHEN 0 THEN 1 ELSE -1 END * l.Valor) as Saldo
                          FROM Contas c
                          JOIN Lancamentos l ON l.ContaId = c.Id
                        WHERE c.Id = @Id
                        GROUP BY c.Id, c.CreatedAt;";
            return base.Query<Conta>(sql, new { Id }).FirstOrDefault();
        }
        public IConta Get(long numeroDaConta)
        {
            var sql = @"SELECT distinct c.Id,
                               c.CreatedAt,
                               SUM(CASE l.Tipo WHEN 0 THEN 1 ELSE -1 END * l.Valor) as Saldo
                          FROM Contas c
                          JOIN Lancamentos l ON l.ContaId = c.Id
                        WHERE c.rowid = @numeroDaConta
                        GROUP BY c.Id, c.CreatedAt;";
            return base.Query<Conta>(sql, new { numeroDaConta }).FirstOrDefault();
        }

        protected override string Fields()
        {
            return "Id, CreatedAt";
        }

        protected override string Table()
        {
            return "Contas";
        }
    }
}
