using PYPA.Transacoes.DataMapping.Interfaces;
using PYPA.Transacoes.Domain.Entities;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Repositories
{
    class UsuarioRepository : BaseRepository<Usuario, IUsuario>, IUsuarioRepository
    {
        public UsuarioRepository(string connectionString) : base(connectionString)
        {
        }

        public UsuarioRepository(string connectionString, SQLiteTransaction sqlTransaction) : base(connectionString, sqlTransaction)
        {
        }

        protected override string Fields()
        {
            return "Id, CreatedAt";
        }

        protected override string Table()
        {
            return "Usuarios";
        }
    }
}
