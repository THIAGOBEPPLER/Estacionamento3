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
    [Route("api/patio")]
    [ApiController]
    public class PatioController : ControllerBase
    {
        EstacionamentoContext bd = new EstacionamentoContext();


        [HttpPut("entrada/{placa}")]
        public ActionResult<string> Entrada(string placa)
        {
            var patio = new Patio();

            var verificaAtivo =
                (from p in bd.Patios
                 where p.veiculoPlaca == placa && p.dataFim == null
                 select p.dataFim).Any();
            //.LastOrDefault();

            if (verificaAtivo)
                return BadRequest("Veiculo ja esta no patio!");

            var dataAtual = DateTime.Now;

            patio.dataInicio = dataAtual;
            patio.veiculoPlaca = placa;

            bd.Add(patio);
            bd.SaveChanges();

            return Ok("Veiculo adicionado ao Patio!");

        }

        [HttpPut("baixa/{placa}")]
        public ActionResult<BaixaModel> Baixa(string placa)
        {
            var veiculo = new Veiculo();
            var patio = new Patio();

            var query =
                (from p in bd.Patios
                 where p.veiculoPlaca == placa && p.dataFim == null
                 select p).SingleOrDefault();

            if (query == null)
                return BadRequest("Esse veiculo nao esta no patio!");

            patio = query;

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

            // var teste = patio.veiculoPlaca;

            var saida =
                (from p in bd.Patios
                 join v in bd.Veiculos on p.veiculoPlaca equals v.placa
                 where v.placa == placa && p.id == patio.id

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
                 }).SingleOrDefault();


            return Ok(saida);

            //var saida =
            //    (from p in bd.Patios
            //     join v in bd.Veiculos on p.veiculoPlaca equals v.placa
            //     where v.placa == placa

            //     select new BaixaModel
            //     {
            //         placa = v.placa,
            //         marca = v.marca,
            //         modelo = v.modelo,
            //         cor = v.cor,
            //         entrada = p.dataInicio,
            //         saida = p.dataFim.Value,
            //         tempo = p.tempo.Value,
            //         valor = p.valor.Value
            //     }).ToList();



            //return Ok(saida.OrderByDescending(d=> d.saida).FirstOrDefault());
        }
    }
}
