using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Exceptions;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Entities
{
    public class Lancamento : Entity, ILancamento
    {
        public TipoDeLancamento Tipo { get; private set; }
        public Guid ContaId { get; private set; }
        public Decimal Valor { get; private set; }
        public DateTime DataDoLancamento { get; private set; }
        public Lancamento(IConta conta, TipoDeLancamento tipo, Decimal valor, DateTime dataDoLancamento, IDateTimeProvider timeProvider) : base(Guid.NewGuid(), timeProvider)
        {
            this.Tipo = tipo;
            this.ContaId = conta.Id;
            this.DataDoLancamento = dataDoLancamento;
            this.DefinirValor(valor);
        }
        protected Lancamento()
        {

        }

        private void DefinirValor(decimal valor)
        {
            if (valor <= 0)
                throw new DomainException("O valor do lançamento é inválido, deve ser maior que zero.");
            this.Valor = valor;
        }
    }
}
