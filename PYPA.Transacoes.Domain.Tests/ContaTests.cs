using FluentAssertions;
using Moq;
using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Entities;
using PYPA.Transacoes.Domain.Exceptions;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PYPA.Transacoes.Domain.Tests
{
    public class ContaTests
    {
        long númeroDaConta = 1234554321;
        decimal valorDeDébito = 15.62m;
        decimal valorDeCrédito = 34.43m;
        Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
        Mock<ILancamento> lançamentoDébitoMock = new Mock<ILancamento>();
        Mock<ILancamento> lançamentoCréditoMock = new Mock<ILancamento>();

        public ContaTests()
        {
            dateTimeProviderMock.SetupGet(tp => tp.Now).Returns(DateTime.Now);
            lançamentoDébitoMock.SetupGet(l => l.Valor).Returns(valorDeDébito);
            lançamentoDébitoMock.SetupGet(l => l.Tipo).Returns(TipoDeLancamento.Debito);
            lançamentoCréditoMock.SetupGet(tp => tp.Valor).Returns(valorDeCrédito);
            lançamentoCréditoMock.SetupGet(tp => tp.Tipo).Returns(TipoDeLancamento.Credito);
        }

        [Fact]
        public void Conta_Deve_Ser_Uma_Entity()
        {
            var conta = new Conta(dateTimeProviderMock.Object);

            conta.Should().BeAssignableTo<Entity>();
        }

        [Fact]
        public void Conta_Deve_Ter_Um_Número()
        {
            var conta = new Conta( dateTimeProviderMock.Object);

            conta.Numero.Should().Be(It.IsAny<long>());
        }

        [Fact]
        public void Conta_Deve_Ser_Criada_Com_Saldo_Zero()
        {
            decimal ZERO = 0;
            var conta = new Conta( dateTimeProviderMock.Object);

            conta.Saldo.Should().Be(ZERO);
        }
        [Fact]
        public void O_Saldo_Da_Conta_Sempre_Deve_Ser_Maior_Ou_Igual_A_Zero()
        {
            var conta = new Conta(dateTimeProviderMock.Object);
            var lancamento = lançamentoCréditoMock.Object;

            conta.AdicionarLancamento(lancamento);

            decimal randomNum = (decimal)new Random(DateTime.Now.Millisecond).NextDouble();
            lançamentoDébitoMock.SetupGet(l => l.Valor).Returns(valorDeCrédito + randomNum);

            conta.Saldo.Should().Be(lancamento.Valor);

            var ex = Assert.Throws<DomainException>(() => conta.AdicionarLancamento(lançamentoDébitoMock.Object));
            ex.Message.Should().Be("O Saldo da conta não pode ser negativo.");
        }
        [Fact]
        public void Deve_Ser_Possível_Adicionar_Lançamentos_Na_Conta_E_O_Saldo_Deve_Ser_Ajustado()
        {
            var conta = new Conta(dateTimeProviderMock.Object);
            var lancamento = lançamentoCréditoMock.Object;

            conta.AdicionarLancamento(lancamento);

            conta.Saldo.Should().Be(lancamento.Valor);
            conta.Saldo.Should().BeGreaterOrEqualTo(0);
        }
    }
}
