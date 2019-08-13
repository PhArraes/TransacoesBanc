using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using System;

namespace PYPA.Transacoes.Domain.Entities
{
    public class Usuario : Entity, IUsuario
    {
        public Usuario(IDateTimeProvider timeProvider) : base(Guid.NewGuid(), timeProvider)
        {
        }

        public Usuario()
        {

        }
    }
}
