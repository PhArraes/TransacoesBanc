using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Entities
{
    public class Transacao : Entity
    {
        public Guid ContaOrigemId { get; private set; }
        public IConta ContaOrigem { get; private set; }
        public Guid ContaDestinoId { get; private set; }
        public IConta ContaDestino { get; private set; }
        public decimal Valor { get; set; }
        public Transacao(IConta contaOrigem, IConta contaDestino, decimal valor, IDateTimeProvider timeProvider) : base(Guid.NewGuid(), timeProvider)
        {
            this.ContaOrigemId = contaOrigem.Id;
            this.ContaOrigem = contaOrigem;
            this.ContaDestinoId = contaDestino.Id;
            this.ContaDestino = contaDestino;
            DefinirValor(valor);
            CriarLancamentoNaContaOrigem(contaOrigem, valor, timeProvider);
            CriarLancamentoNaContaDestino(contaDestino, valor, timeProvider);
        }

        private void CriarLancamentoNaContaOrigem(IConta conta, decimal valor, IDateTimeProvider timeProvider) {
            var lancamento = new Lancamento(conta, TipoDeLancamento.Debito, valor, this.CreatedAt, timeProvider);
            conta.AdicionarLancamento(lancamento);
        }


        private void CriarLancamentoNaContaDestino(IConta conta, decimal valor, IDateTimeProvider timeProvider)
        {
            var lancamento = new Lancamento(conta, TipoDeLancamento.Credito, valor, this.CreatedAt, timeProvider);
            conta.AdicionarLancamento(lancamento);
        }

        private void DefinirValor(decimal valor)
        {
            if (valor <= 0) throw new ArgumentException("O valor da transação deve ser maior que zero.");
            this.Valor = valor;
        }
    }
}
