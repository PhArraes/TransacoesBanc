using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Database.Dapper
{
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override Guid Parse(object value)
        {
            return new Guid((string)value);
        }

        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            parameter.Value = value.ToString();
        }
    }
}
