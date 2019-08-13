using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using Dapper;
using PYPA.Transacoes.Domain.Entities;
using PYPA.Transacoes.Domain.Interfaces.Entities;

namespace PYPA.Transacoes.DataMapping
{
    class ContaLockRepository : BaseRepository<ContaLock, IContaLock>, IContaLockRepository
    {
        public ContaLockRepository(string connectionString) : base(connectionString)
        {
        }

        public ContaLockRepository(string connectionString, SQLiteTransaction sqlTransaction) : base(connectionString, sqlTransaction)
        {
        }

        public bool GetAndSetLock(Guid Id)
        {
            var sql = @"UPDATE Contas SET Lock = 1
                            where Id = @Id AND Lock = 0";
            var res = this.Execute(sql, new { Id });
            return res > 0;
        }

        public bool GetAndSetLock(long numeroConta)
        {
            var sql = @"UPDATE Contas SET Lock = 1
                            where rowid = @numeroConta AND Lock = 0";
            var res = this.Execute(sql, new { numeroConta });
            return res > 0;
        }

        public bool ReleaseLock(long numeroConta)
        {
            var sql = @"UPDATE Contas SET Lock = 0
                            where rowid = @numeroConta AND Lock = 1";
            var res = this.Execute(sql, new { numeroConta });
            return res > 0;
        }

        public bool ReleaseLock(Guid Id)
        {
            var sql = @"UPDATE Contas SET Lock = 0
                            where Id = @Id AND Lock = 1";
            var res = this.Execute(sql, new { Id });
            return res > 0;
        }

        protected override string Fields()
        {
            return "Id, Lock";
        }
        protected override string Table()
        {
            return "Contas";
        }
    }
}
