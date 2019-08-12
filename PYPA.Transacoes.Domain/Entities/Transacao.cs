using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PYPA.Transacoes.Domain.Entities
{
    public class Transacao : Entity, ITransacao
    {
        public Guid UsuarioResponsavelId { get; private set; }
        public IUsuario UsuarioResponsavel { get; private set; }
        public Guid ContaOrigemId { get; private set; }
        public IConta ContaOrigem { get; private set; }
        public List<Guid> ContasDestinoIds { get; private set; }
        public List<IConta> ContasDestino { get; private set; }
        public decimal Valor { get; set; }
        public Transacao(IUsuario usuario, IConta contaOrigem, List<IConta> contasDestino, decimal valor, IDateTimeProvider timeProvider) : base(Guid.NewGuid(), timeProvider)
        {
            this.UsuarioResponsavelId = usuario.Id;
            this.UsuarioResponsavel = usuario;
            this.ContaOrigemId = contaOrigem.Id;
            this.ContaOrigem = contaOrigem;
            this.ContasDestinoIds = contasDestino.Select(c => c.Id).ToList();
            this.ContasDestino = contasDestino;
            var valorTotal = valor * contasDestino.Count;
            DefinirValor(valorTotal);
            ContasDestino.ForEach(c =>
            {
                CriarLancamentoNaContaOrigem(contaOrigem, valor, timeProvider);
                CriarLancamentoNaContaDestino(c, valor, timeProvider);
            });
        }

        private void CriarLancamentoNaContaOrigem(IConta conta, decimal valor, IDateTimeProvider timeProvider)
        {
            var lancamento = new Lancamento(this, conta, TipoDeLancamento.Debito, valor, this.CreatedAt, timeProvider);
            conta.AdicionarLancamento(lancamento);
        }


        private void CriarLancamentoNaContaDestino(IConta conta, decimal valor, IDateTimeProvider timeProvider)
        {
            var lancamento = new Lancamento(this, conta, TipoDeLancamento.Credito, valor, this.CreatedAt, timeProvider);
            conta.AdicionarLancamento(lancamento);
        }

        private void DefinirValor(decimal valor)
        {
            if (valor <= 0) throw new ArgumentException("O valor da transação deve ser maior que zero.");
            this.Valor = valor;
        }
    }
}
