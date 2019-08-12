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
        Guid usuarioId = Guid.NewGuid();
        List<Guid> contasDestinoIds = new List<Guid>();
        decimal valor = 123m;
        DateTime now = DateTime.Now;
        Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
        Mock<IConta> contaOrigemMock = new Mock<IConta>();
        Mock<IUsuario> usuarioMock = new Mock<IUsuario>();
        Mock<IConta> contaDestinoMock = new Mock<IConta>();
        Mock<IConta> contaDestino2Mock = new Mock<IConta>();
        Mock<IConta> contaDestino3Mock = new Mock<IConta>();

        List<IConta> umaContaDestino = new List<IConta>();
        List<IConta> contasDestino = new List<IConta>();

        public TransacaoTests()
        {
            usuarioMock.SetupGet(c => c.Id).Returns(usuarioId);

            contasDestinoIds.Add(Guid.NewGuid());
            contasDestinoIds.Add(Guid.NewGuid());
            contasDestinoIds.Add(Guid.NewGuid());
            dateTimeProviderMock.SetupGet(tp => tp.Now).Returns(now);
            contaOrigemMock.SetupGet(c => c.Id).Returns(contaOrigemId);

            contaDestinoMock.SetupGet(c => c.Id).Returns(contasDestinoIds[0]);
            umaContaDestino.Add(contaDestinoMock.Object);
            contasDestino.Add(umaContaDestino.First());
            contaDestino2Mock.SetupGet(c => c.Id).Returns(contasDestinoIds[1]);
            contasDestino.Add(contaDestino2Mock.Object);
            contaDestino3Mock.SetupGet(c => c.Id).Returns(contasDestinoIds[2]);
            contasDestino.Add(contaDestino3Mock.Object);
        }

        [Fact]
        public void Transacao_Deve_Ser_Uma_Entity()
        {
            var transacao = new Transacao(usuarioMock.Object,contaOrigemMock.Object,
                                        umaContaDestino,
                                        valor,
                                        dateTimeProviderMock.Object);

            transacao.Should().BeAssignableTo<Entity>();
        }

        [Fact]
        public void Trasacao_Deve_Ter_Um_Usuario_Responsavel()
        {
            var usuario = usuarioMock.Object;
            var transacao = new Transacao(usuario, contaOrigemMock.Object,
                                        umaContaDestino,
                                        valor,
                                        dateTimeProviderMock.Object);

            transacao.UsuarioResponsavelId.Should().Be(contaOrigemId);
            transacao.UsuarioResponsavel.Should().BeSameAs(usuario);
        }

        [Fact]
        public void Trasacao_Deve_Ter_Uma_Conta_Origem()
        {
            var contaOrigem = contaOrigemMock.Object;
            var transacao = new Transacao(usuarioMock.Object,contaOrigem,
                                        umaContaDestino,
                                        valor,
                                        dateTimeProviderMock.Object);

            transacao.ContaOrigemId.Should().Be(contaOrigemId);
            transacao.ContaOrigem.Should().BeSameAs(contaOrigem);
        }

        [Fact]
        public void Trasacao_Deve_Ter_Uma_Ou_Mais_Contas_Destino()
        {
            var contaDestino = umaContaDestino[0];
            var transacao = new Transacao(usuarioMock.Object,contaOrigemMock.Object,
                                        umaContaDestino,
                                        valor,
                                        dateTimeProviderMock.Object);

            transacao.ContasDestino.First().Id.Should().Be(contasDestinoIds[0]);
            transacao.ContasDestino.First().Should().BeSameAs(contaDestino);

            contaDestino = contaDestinoMock.Object;
            transacao = new Transacao(usuarioMock.Object,contaOrigemMock.Object,
                                        contasDestino,
                                        valor,
                                        dateTimeProviderMock.Object);
            var i = 0;
            transacao.ContasDestino.ForEach(c =>
            {
                c.Id.Should().Be(contasDestinoIds[i]);
                c.Should().BeSameAs(contasDestino[i]);
                i++;
            });
        }
        [Fact]
        public void Trasacao_Deve_Ter_Um_Valor_Positivo_Maior_Que_Zero_Para_Ser_Transferido()
        {
            var contaDestino = contaDestinoMock.Object;
            decimal valor = 123m;
            var transacao = new Transacao(usuarioMock.Object,contaOrigemMock.Object,
                                        umaContaDestino,
                                        valor,
                                        dateTimeProviderMock.Object);

            transacao.Valor.Should().Be(valor);
            transacao.Valor.Should().BeGreaterThan(0);

            valor = 0;
            var ex = Assert.Throws<ArgumentException>(() => new Transacao(usuarioMock.Object,contaOrigemMock.Object,
                                        umaContaDestino,
                                        valor,
                                        dateTimeProviderMock.Object));

            ex.Message.Should().Be("O valor da transação deve ser maior que zero.");

        }

        [Fact]
        public void Transacao_Deve_Adicionar_Um_Lancamento_De_Debito_Para_Cada_Conta_Destino_Na_Conta_Origem_Com_Valor_Igual_Ao_Valor_Da_Transacao()
        {
            var lancamentos = new List<ILancamento>();
            var contaOrigem = contaOrigemMock.Object;
            decimal valor = 123m;

            contaOrigemMock.Setup(c => c.AdicionarLancamento(It.IsAny<ILancamento>())).Callback<ILancamento>(l =>
            {
                lancamentos.Add(l);
            });

            var transacao = new Transacao(usuarioMock.Object,contaOrigem,
                                       umaContaDestino,
                                        valor,
                                        dateTimeProviderMock.Object);

            var lancamento = lancamentos.FirstOrDefault();
            lancamento.Should().NotBeNull();
            lancamento.CreatedAt.Should().Be(now);
            lancamento.DataDoLancamento.Should().Be(now);
            lancamento.ContaId.Should().Be(contaOrigemId);
            lancamento.Valor.Should().Be(valor);
            lancamento.Tipo.Should().Be(TipoDeLancamento.Debito);

            lancamentos = new List<ILancamento>();
            contaOrigem = contaOrigemMock.Object;
            valor = 123m;

            transacao = new Transacao(usuarioMock.Object,contaOrigem,
                                      contasDestino,
                                       valor,
                                       dateTimeProviderMock.Object);

            lancamentos.Count.Should().Be(contasDestino.Count);
            lancamentos.ForEach(l =>
            {
                l.Should().NotBeNull();
                l.CreatedAt.Should().Be(now);
                l.DataDoLancamento.Should().Be(now);
                l.ContaId.Should().Be(contaOrigemId);
                l.Valor.Should().Be(valor);
                l.Tipo.Should().Be(TipoDeLancamento.Debito);
                l.TransacaoId.Should().Be(transacao.Id);
            });

            transacao.Valor.Should().Be(valor * contasDestino.Count);
        }

        [Fact]
        public void Transacao_Deve_Adicionar_Um_Lancamento_De_Credito_Na_Conta_Destino_Com_Valor_Igual_Ao_Valor_Da_Transacao()
        {
            var lancamentos = new List<ILancamento>();
            var contaDestinoId = Guid.NewGuid();
            decimal valor = 123m;

            contaDestinoMock.SetupGet(c => c.Id).Returns(contaDestinoId);
            contaDestinoMock.Setup(c => c.AdicionarLancamento(It.IsAny<ILancamento>())).Callback<ILancamento>(l =>
            {
                lancamentos.Add(l);
            });
            var contaDestino = contaDestinoMock.Object;

            var transacao = new Transacao(usuarioMock.Object,contaOrigemMock.Object,
                                        umaContaDestino,
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

            var ex = Assert.Throws<Exception>(() => new Transacao(usuarioMock.Object,contaOrigem,
                                        umaContaDestino,
                                        valor,
                                        dateTimeProviderMock.Object));
        }
    }
}
