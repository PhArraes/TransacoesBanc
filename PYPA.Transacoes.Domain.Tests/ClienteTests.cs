using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using PYPA.Transacoes.Domain.Entities;
using PYPA.Transacoes.Domain.Core;
using Moq;
using PYPA.Transacoes.Domain.Interfaces.Core;

namespace PYPA.Transacoes.Domain.Tests
{
    public class ClienteTests
    {
        Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
        string document = null;
        public ClienteTests()
        {
             document = "Um Documento";
            dateTimeProviderMock.SetupGet(tp => tp.Now).Returns(DateTime.Now);
        }

        [Fact]
        public void Cliente_Deve_Ser_Uma_Entity()
        {
            var Cliente = new Cliente(document, dateTimeProviderMock.Object);

            Cliente.Should().BeAssignableTo<Entity>();
        }

        [Fact]
        public void Cliente_Deve_Ser_Criado_Com_Um_Documento()
        {
            var Cliente = new Cliente(document, dateTimeProviderMock.Object);

            Cliente.Documento.Should().Be(document);
        }
    }
}
