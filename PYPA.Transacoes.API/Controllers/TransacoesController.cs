using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PYPA.Transacoes.DataMapping.Factories;
using PYPA.Transacoes.Domain.Exceptions;
using PYPA.Transacoes.Facade;
using PYPA.Transacoes.Facade.Models;

namespace PYPA.Transacoes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransacoesController : ControllerBase
    {
        TransacaoService transacaoService;
        TransacaoRepositoryFactory transacaoRepositoryFactory;
        ILogger<TransacoesController> logger;

        public TransacoesController(TransacaoService transacaoService, TransacaoRepositoryFactory transacaoRepositoryFactory,
            ILogger<TransacoesController> logger)
        {
            this.transacaoService = transacaoService;
            this.transacaoRepositoryFactory = transacaoRepositoryFactory;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(transacaoService.ListAll());
        }


        [HttpGet("{id}/lancamentos")]
        public IActionResult Lancamentos(Guid id)
        {
            return Ok(transacaoRepositoryFactory.Create().Lancamentos(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] NovaTransacaoRequest request)
        {
            try
            {
                var id = transacaoService.NovaTransacao(request);
                ExecAsync(() => logger.LogInformation("Transação efetuada com sucesso", request));
                return Ok(id);
            }
            catch (DomainException ex)
            {
                ExecAsync(() => logger.LogError(ex, $"Transação falhou: {ex.Message}", request));
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                ExecAsync(()=> logger.LogError(e, $"Transação falhou: {e.Message}", request));
                throw e;
            }
        }

        private void ExecAsync(Action act)
        {
            Thread thread = new Thread(() =>
            {
                act();
            });
            thread.Start();
        }
    }
}
