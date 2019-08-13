using Dapper;
using PYPA.Transacoes.DataMapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace PYPA.Transacoes.DataMapping
{
    abstract class BaseRepository<Tp, Tpr> : IBaseRepository<Tpr>
         where Tp : Tpr
        where Tpr : class
    {
        private IDbTransaction sqlTransaction;

        public BaseRepository(String connectionString, SQLiteTransaction sqlTransaction) : this(connectionString)
        {
            this.sqlTransaction = sqlTransaction;
        }

        public BaseRepository(String connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private bool IsValidTransaction()
        {
            return (this.sqlTransaction != null && this.sqlTransaction.Connection != null);
        }

        public String ConnectionString { get; set; }

        public virtual Tpr Get(Guid Id)
        {
            var sql = $@"SELECT {Fields()} FROM {Table()} WHERE Id=@Id";
            return this.Query<Tp>(sql, new { Id }).First();
        }
        public virtual IEnumerable<Tpr> ListAll()
        {
            var sql = $@"SELECT {Fields()} FROM {Table()} ";
            return Query<Tp>(sql).Select(t => (Tpr)t);
        }

        protected abstract String Fields();
        protected abstract String Table();

        protected IEnumerable<T> Query<T>(String sql)
        {
            return this.Query<T>(sql, new { });
        }

        protected IEnumerable<T> Query<T>(String sql, object param)
        {
            if (IsValidTransaction())
                return sqlTransaction.Connection.Query<T>(sql, param, this.sqlTransaction);

            using (IDbConnection db = new SQLiteConnection(ConnectionString))
            {
                return db.Query<T>(sql, param);
            }
        }

        protected Int32 Execute(String sql, object param = null)
        {
            if (IsValidTransaction())
                return sqlTransaction.Connection.Execute(sql, param, this.sqlTransaction);

            using (IDbConnection db = new SQLiteConnection(ConnectionString))
            {
                return db.Execute(sql, param);
            }
        }
    }
}
