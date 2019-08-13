using PYPA.Transacoes.DataMapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PYPA.Transacoes.DataMapping.Database
{
    public class DbConfiguration 
    {
        public string ConnectionString { get; set; }

        public string File { get; set; }
    }
}