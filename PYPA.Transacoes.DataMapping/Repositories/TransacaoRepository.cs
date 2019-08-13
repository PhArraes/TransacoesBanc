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
    class TransacaoRepository : BaseRepository<Transacao, ITransacao>, ITransacaoRepository
    {
        public TransacaoRepository(string connectionString) : base(connectionString)
        {
        }

        public TransacaoRepository(string connectionString, SQLiteTransaction sqlTransaction) : base(connectionString, sqlTransaction)
        {
        }


        public IEnumerable<ILancamento> Lancamentos(Guid transacaoId)
        {
            var sql = @"SELECT  l.Id, l.CreatedAt, l.ContaId, l.DataDoLancamento, l.Tipo, l.Valor 
                        FROM Lancamentos l
                        INNER JOIN Transacoes_Lancamentos t on t.TransacaoId = @id AND t.LancamentoId = l.Id";
            return Query<Lancamento>(sql, new { id = transacaoId });
        }

        public void Save(ITransacao transacao)
        {
            transacao.Lancamentos.ForEach(l => SaveLancamento(l));
            SaveTransacao(transacao);
            SaveTransacaoLancamento(transacao);
        }

        private void SaveLancamento(ILancamento lancamento) {
            var sql = @"
            INSERT INTO Lancamentos (Id, CreatedAt, Valor, DataDoLancamento, ContaId, Tipo)
            VALUES (@Id, @CreatedAt, @Valor, @DataDoLancamento, @ContaId, @Tipo)";
            this.Execute(sql, lancamento);
        }
        private void SaveTransacao(ITransacao transacao )
        {
            var sql = @"
            INSERT INTO Transacoes (Id, CreatedAt, ContaOrigemId, Valor, UsuarioResponsavelId)
            VALUES (@Id, @CreatedAt, @ContaOrigemId, @Valor, @UsuarioResponsavelId)";
            this.Execute(sql, transacao);
        }
        private void SaveTransacaoLancamento(ITransacao transacao)
        {
            var sql = @"
            INSERT INTO Transacoes_Lancamentos (TransacaoId, LancamentoId)
            VALUES (@TransacaoId, @LancamentoId)";
            transacao.Lancamentos.ForEach(l => this.Execute(sql, new { TransacaoId = transacao.Id, LancamentoId = l.Id }));
        }
        protected override string Fields()
        {
            return "Id,CreatedAt,ContaOrigemId,UsuarioResponsavelId, Valor";
        }

        protected override string Table()
        {
            return "Transacoes";
        }
    }
}
