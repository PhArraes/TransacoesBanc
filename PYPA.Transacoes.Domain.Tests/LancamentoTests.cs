using FluentAssertions;
using Moq;
using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Entities;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PYPA.Transacoes.Domain.Tests
{
    public class LancamentoTests
    {
        TipoDeLancamento credito = TipoDeLancamento.Credito;
        TipoDeLancamento debito = TipoDeLancamento.Debito;
        Guid contaId = Guid.NewGuid();
        decimal valor = 1514.341m;

        Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
        Mock<IConta> contaMock = new Mock<IConta>();

        public LancamentoTests()
        {
            dateTimeProviderMock.SetupGet(tp => tp.Now).Returns(DateTime.Now);
            contaMock.SetupGet(c => c.Id).Returns(contaId);
        }

        [Fact]
        public void Lancamento_Deve_Ter_Uma_Conta()
        {
            var lancamento = new Lancamento(contaMock.Object, credito, valor, dateTimeProviderMock.Object);

            lancamento.ContaId.Should().Be(contaId);
        }

        [Fact]
        public void Lancamento_Deve_Ter_Um_Valor()
        {
            var lancamento = new Lancamento(contaMock.Object, credito, valor, dateTimeProviderMock.Object);

            lancamento.Valor.Should().Be(valor);
        }

        [Fact]
        public void O_Valor_Do_Lancamento_Deve_Ser_Maior_Que_Zero()
        {
            valor = -1;
            var ex = Assert.Throws<ArgumentException>(() => new Lancamento(contaMock.Object, credito, valor, dateTimeProviderMock.Object));

            ex.Message.Should().Be("O valor do lançamento é inválido, deve ser maior que zero.");

            valor = 10;
            var lancamento = new Lancamento(contaMock.Object, credito, valor, dateTimeProviderMock.Object);

            lancamento.Valor.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Lancamento_Deve_Ter_Um_Tipo()
        {
            var lancamento = new Lancamento(contaMock.Object, credito, valor, dateTimeProviderMock.Object);

            lancamento.Tipo.Should().Be(credito);
        }

        [Fact]
        public void Lancamento_Deve_Ser_Uma_Entidade()
        {
            var lancamento = new Lancamento(contaMock.Object, debito, valor, dateTimeProviderMock.Object);

            lancamento.Should().BeAssignableTo<Entity>();
        }
    }
}
