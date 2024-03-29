﻿using PYPA.Transacoes.Domain.Exceptions;
using PYPA.Transacoes.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.Domain.Core
{
    public class Entity
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Entity(Guid id, IDateTimeProvider timeProvider)
        {
            SetId(id);
            CreatedAt = timeProvider.Now;
        }
        protected Entity() { }

        private void SetId(Guid id)
        {
            if (id == Guid.Empty)
                throw new DomainException("Entity created with invalid Empty Id");
            this.Id = id;
        }
    }
}
