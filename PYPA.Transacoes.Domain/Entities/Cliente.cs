using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Entities
{
    public class Cliente : Entity
    {
        public string Documento { get; private set; }
        public Cliente(string documento, IDateTimeProvider timeProvider) : base(Guid.NewGuid(), timeProvider)
        {
            this.Documento = documento;
        }
    }
}
