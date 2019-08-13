using System;
using Xunit;
using FluentAssertions;
using PYPA.Transacoes.Domain.Core;
using Moq;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Domain.Exceptions;

namespace PYPA.Transacoes.Domain.Tests
{
    public class EntityTests
    {
        DateTime now = DateTime.Now;
        Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();

        public EntityTests()
        {
            dateTimeProviderMock.SetupGet(tp => tp.Now).Returns(now);
        }

        [Fact]
        public void Entity_Deve_Ser_Criado_Com_Id_E_DataDeCriação()
        {
            Guid id = Guid.NewGuid();

            var entity = new Entity(id, dateTimeProviderMock.Object);

            entity.Id.Should().Be(id);
            entity.CreatedAt.Should().Be(now);
        }

        [Fact]
        public void Entity_Deve_Ser_Criado_Com_Id_Não_Nulo()
        {

            var ex = Assert.Throws<DomainException>(() => new Entity(Guid.Empty, dateTimeProviderMock.Object));
            ex.Message.Should().Be("Entity created with invalid Empty Id");
        }
    }
}
