using PYPA.Transacoes.DataMapping.Factories;
using PYPA.Transacoes.Domain.Entities;
using PYPA.Transacoes.Domain.Exceptions;
using PYPA.Transacoes.Domain.Factories;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Domain.Interfaces.Entities;
using PYPA.Transacoes.Facade.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PYPA.Transacoes.Facade
{
    public class TransacaoService
    {
        ContaLockService contaLockService;
        IDateTimeProvider dateTimeProvider;
        TransacaoFactory transacaoFactory;
        ContaRepositoryFactory contaRepositoryFactory;
        UsuarioRepositoryFactory usuarioRepositoryFactory;
        TransacaoRepositoryFactory transacaoRepositoryFactory;
        TransactionFactory transactionFactory;

        public TransacaoService(ContaLockService contaLockService, IDateTimeProvider dateTimeProvider, TransacaoFactory transacaoFactory, ContaRepositoryFactory contaRepositoryFactory, UsuarioRepositoryFactory usuarioRepositoryFactory, TransacaoRepositoryFactory transacaoRepositoryFactory, TransactionFactory transactionFactory)
        {
            this.contaLockService = contaLockService;
            this.dateTimeProvider = dateTimeProvider;
            this.transacaoFactory = transacaoFactory;
            this.contaRepositoryFactory = contaRepositoryFactory;
            this.usuarioRepositoryFactory = usuarioRepositoryFactory;
            this.transacaoRepositoryFactory = transacaoRepositoryFactory;
            this.transactionFactory = transactionFactory;
        }


        public IEnumerable<ITransacao> ListAll()
        {
            var repo = transacaoRepositoryFactory.Create();
            return repo.ListAll();
        }

        public Guid NovaTransacao(NovaTransacaoRequest request)
        {
            var contaOrigem = ObterConta(request.ContaOrigem);
            if (contaLockService.PegarLock(request.ContaOrigem))
            {
                try
                {
                    var usuario = ObterUsuarioResponsavel(request);
                    var contasDestino = ObterContasDestino(request.ContasDestino);
                    var transacao = CriarTransacao(usuario, contaOrigem, contasDestino, request.Valor);
                    Salvar(transacao);
                    return transacao.Id;
                }
                catch (Exception e) { throw e; }
                finally { contaLockService.LiberarLock(request.ContaOrigem); }
            }
            else
            {
                throw new DomainException("Não foi possível concluir a operação pois a conta está bloqueada em outra operação");
            }
        }

        private IConta ObterConta(long numero)
        {
            var conta = contaRepositoryFactory.Create().Get(numero);
            if (conta == null) throw new DomainException($"Conta Nº{numero} não encontrada.");
            return conta;
        }
        private List<IConta> ObterContasDestino(List<long> numeros)
        {
            var repo = contaRepositoryFactory.Create();
            return numeros.Select(n =>
            {
                var conta = repo.Get(n);
                if (conta == null) throw new DomainException($"Conta Nº{n} não encontrada.");
                return conta;
            }).ToList();
        }
        private IUsuario ObterUsuarioResponsavel(NovaTransacaoRequest request)
        {
            var usuario = usuarioRepositoryFactory.Create().Get(request.Usuario);
            if (usuario == null) throw new DomainException("Usuário não encontrado.");
            return usuario;
        }
        private ITransacao CriarTransacao(IUsuario usuario, IConta contaOrigem, List<IConta> contasDestino, decimal valor)
        {
            return transacaoFactory.Create(usuario, contaOrigem, contasDestino, valor);
        }

        private void Salvar(ITransacao transacao)
        {
            var transaction = transactionFactory.CreateSQLTransaction();
            try
            {
                var repo = transacaoRepositoryFactory.Create(transaction);
                repo.Save(transacao);
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
            finally
            {
                transaction.Connection?.Close();
            }
        }
    }
}
