using FluentAssertions;
using Moq;
using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Entities;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PYPA.Transacoes.Domain.Tests
{
    public class TransacaoTests
    {
        Guid contaOrigemId = Guid.NewGuid();
        Guid contaDestinoId = Guid.NewGuid();
        decimal valor = 123m;
        DateTime now = DateTime.Now;
        Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
        Mock<IConta> contaOrigemMock = new Mock<IConta>();
        Mock<IConta> contaDestinoMock = new Mock<IConta>();

        public TransacaoTests()
        {
            dateTimeProviderMock.SetupGet(tp => tp.Now).Returns(now);
            contaOrigemMock.SetupGet(c => c.Id).Returns(contaOrigemId);
            contaDestinoMock.SetupGet(c => c.Id).Returns(contaDestinoId);
        }

        [Fact]
        public void Transacao_Deve_Ser_Uma_Entity()
        {
            var transacao = new Transacao(contaOrigemMock.Object,
                                        contaDestinoMock.Object,
                                        valor,
                                        dateTimeProviderMock.Object);

            transacao.Should().BeAssignableTo<Entity>();
        }

        [Fact]
        public void Trasacao_Deve_Ter_Uma_Conta_Origem()
        {
            var contaOrigem = contaOrigemMock.Object;
            var transacao = new Transacao(contaOrigem,
                                        contaDestinoMock.Object,
                                        valor,
                                        dateTimeProviderMock.Object);

            transacao.ContaOrigemId.Should().Be(contaOrigemId);
            transacao.ContaOrigem.Should().BeSameAs(contaOrigem);
        }

        [Fact]
        public void Trasacao_Deve_Ter_Uma_Conta_Destino()
        {
            var contaDestino = contaDestinoMock.Object;
            var transacao = new Transacao(contaOrigemMock.Object,
                                        contaDestino,
                                        valor,
                                        dateTimeProviderMock.Object);

            transacao.ContaDestinoId.Should().Be(contaDestinoId);
            transacao.ContaDestino.Should().BeSameAs(contaDestino);
        }
        [Fact]
        public void Trasacao_Deve_Ter_Um_Valor_Positivo_Maior_Que_Zero_Para_Ser_Transferido()
        {
            var contaDestino = contaDestinoMock.Object;
            decimal valor = 123m;
            var transacao = new Transacao(contaOrigemMock.Object,
                                        contaDestino,
                                        valor,
                                        dateTimeProviderMock.Object);

            transacao.Valor.Should().Be(valor);
            transacao.Valor.Should().BeGreaterThan(0);

            valor = 0;
            var ex = Assert.Throws<ArgumentException>(() => new Transacao(contaOrigemMock.Object,
                                        contaDestino,
                                        valor,
                                        dateTimeProviderMock.Object));

            ex.Message.Should().Be("O valor da transação deve ser maior que zero.");
            
        }

        [Fact]
        public void Transacao_Deve_Adicionar_Um_Lancamento_De_Debito_Na_Conta_Origem_Com_Valor_Igual_Ao_Valor_Da_Transacao()
        {
            var lancamentos = new List<ILancamento>();
            var contaOrigem = contaOrigemMock.Object;
            decimal valor = 123m;

            contaOrigemMock.Setup(c => c.AdicionarLancamento(It.IsAny<ILancamento>())).Callback<ILancamento>(l => {
                lancamentos.Add(l);
            });

            var transacao = new Transacao(contaOrigem,
                                        contaDestinoMock.Object,
                                        valor,
                                        dateTimeProviderMock.Object);

            var lancamento = lancamentos.FirstOrDefault();
            lancamento.Should().NotBeNull();
            lancamento.CreatedAt.Should().Be(now);
            lancamento.DataDoLancamento.Should().Be(now);
            lancamento.ContaId.Should().Be(contaOrigemId);
            lancamento.Valor.Should().Be(valor);
            lancamento.Tipo.Should().Be(TipoDeLancamento.Debito);
        }

        [Fact]
        public void Transacao_Deve_Adicionar_Um_Lancamento_De_Credito_Na_Conta_Destino_Com_Valor_Igual_Ao_Valor_Da_Transacao()
        {
            var lancamentos = new List<ILancamento>();
            var contaDestino = contaDestinoMock.Object;
            decimal valor = 123m;

            contaDestinoMock.Setup(c => c.AdicionarLancamento(It.IsAny<ILancamento>())).Callback<ILancamento>(l => {
                lancamentos.Add(l);
            });

            var transacao = new Transacao(contaOrigemMock.Object,
                                        contaDestino,
                                        valor,
                                        dateTimeProviderMock.Object);

            var lancamento = lancamentos.FirstOrDefault();
            lancamento.Should().NotBeNull();
            lancamento.CreatedAt.Should().Be(now);
            lancamento.DataDoLancamento.Should().Be(now);
            lancamento.ContaId.Should().Be(contaDestinoId);
            lancamento.Valor.Should().Be(valor);
            lancamento.Tipo.Should().Be(TipoDeLancamento.Credito);
        }
        [Fact]
        public void A_Trannsacao_Nao_Deve_Ser_Criada_Se_A_Conta_Origem_Nao_Tiver_Saldo()
        {
            var lancamentos = new List<ILancamento>();
            var contaOrigem = contaOrigemMock.Object;
            decimal valor = 123m;
            decimal saldoContaOrigem = 100m;

            contaOrigemMock.SetupGet(c => c.Saldo).Returns(saldoContaOrigem);

            contaOrigemMock.Setup(c => c.AdicionarLancamento(It.IsAny<ILancamento>())).Throws(new Exception("Lançamento inválido"));
            
            var ex = Assert.Throws<Exception>(() => new Transacao(contaOrigem,
                                        contaDestinoMock.Object,
                                        valor,
                                        dateTimeProviderMock.Object));
        }
    }
}
