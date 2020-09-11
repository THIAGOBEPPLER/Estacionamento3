using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estacionamento3.Data;
using Estacionamento3.Entities;
using Estacionamento3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento3.Controllers
{
    [Route("api/Saida")]
    [ApiController]
    public class SaidaController : ControllerBase
    {
        EstacionamentoContext bd = new EstacionamentoContext();

        [HttpGet("Ativos")]
        public List<AtivosModel> Get()
        {
            var query =
                (from p in bd.Patios
                 join v in bd.Veiculos on p.veiculoPlaca equals v.placa
                 where p.dataFim == null
                 select new AtivosModel { placa = v.placa, marca = v.marca, modelo = v.modelo, cor = v.cor, entrada = p.dataInicio }).ToList();

            return query;
        }

        [HttpPut("Baixa/{placa}")]
        public ActionResult<BaixaModel> Put(string placa)
        {
            var veiculo = new Veiculo();
            var patio = new Patio();

            var query =
                (from p in bd.Patios
                 where p.veiculoPlaca == placa && p.dataFim == null
                 select p);

            if (!query.Any())
                return BadRequest("Esse veiculo nao esta no patio!");

            patio = query.SingleOrDefault();

            var dataAtual = DateTime.Now;
            double duracao;
            float valor;
            TimeSpan castDuracao;
            DateTime dataIni;

            dataIni = patio.dataInicio;

            castDuracao = dataAtual - dataIni;

            duracao = (int)castDuracao.TotalMinutes;

            if (duracao <= 30)
                valor = 5;
            else
            {
                valor = (int)(duracao / 30) * 5;
            }

            patio.tempo = duracao;
            patio.dataFim = dataAtual;
            patio.valor = valor;

            bd.Update(patio);
            bd.SaveChanges();

            var saida =
                (from p in bd.Patios
                 join v in bd.Veiculos on p.veiculoPlaca equals v.placa
                 where v.placa == placa
                 
                 select new BaixaModel
                 {
                     placa = v.placa,
                     marca = v.marca,
                     modelo = v.modelo,
                     cor = v.cor,
                     entrada = p.dataInicio,
                     saida = p.dataFim.Value,
                     tempo = p.tempo.Value,
                     valor = p.valor.Value
                 }).ToList();



            return Ok(saida.OrderByDescending(d=> d.saida).FirstOrDefault());
        }
    }
}
