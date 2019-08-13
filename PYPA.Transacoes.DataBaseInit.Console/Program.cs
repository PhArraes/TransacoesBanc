using PYPA.Transacoes.DataMapping.Database;
using System;

namespace PYPA.Transacoes.DataBaseInit.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbInit = new DatabaseInit();
            dbInit.Init();
        }
    }
}
