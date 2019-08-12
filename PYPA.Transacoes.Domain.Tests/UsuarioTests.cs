using FluentAssertions;
using Moq;
using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Entities;
using PYPA.Transacoes.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PYPA.Transacoes.Domain.Tests
{
    public class UsuarioTests
    {
        Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
        [Fact]
        public void Usuário_Deve_Ser_Uma_Entity ()
        {
            var usuario = new Usuario(dateTimeProviderMock.Object);

            usuario.Should().BeAssignableTo<Entity>();

        }
    }
}
