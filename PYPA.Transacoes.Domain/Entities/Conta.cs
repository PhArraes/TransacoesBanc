using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Entities
{
    public class Conta : Entity, IConta
    {
        public long Numero { get; private set; }
        public decimal Saldo { get; private set; }

        public IReadOnlyList<ILancamento> Lancamentos
        {
            get { return lancamentos.AsReadOnly(); }
        }

        private List<ILancamento> lancamentos;
        public Conta(IDateTimeProvider timeProvider) : base(Guid.NewGuid(), timeProvider)
        {
            this.Saldo = 0;
            this.lancamentos = new List<ILancamento>();
        }

        public Conta()
        {

        }

        public void AdicionarLancamento(ILancamento lancamento)
        {
            var novoSaldo = CalcularNovoSaldo(lancamento);
            DefinirSaldo(novoSaldo);
            this._AdicionarLancamento(lancamento);
        }

        private void _AdicionarLancamento(ILancamento lancamento)
        {
            if (lancamentos == null) lancamentos = new List<ILancamento>();
            lancamentos.Add(lancamento);
        }

        private Decimal CalcularNovoSaldo(ILancamento lancamento)
        {
            var fator = lancamento.Tipo == TipoDeLancamento.Credito ? 1 : -1;
            return Saldo + (fator * lancamento.Valor);            
        }

        private void DefinirSaldo(decimal valor)
        {
            if (valor < 0)
                throw new Exception("O Saldo da conta não pode ser negativo.");
            this.Saldo = valor;
        }
    }
}
