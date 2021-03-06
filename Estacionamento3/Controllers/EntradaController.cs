﻿using System;
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
    // [RoutePrefix("api/books")]
    [Route("api/Entrada")]
    [ApiController]
    public class EntradaController : ControllerBase
    {
        EstacionamentoContext bd = new EstacionamentoContext();

        [HttpGet("VerificaPlaca/{placa}")]
        public ActionResult<VerificaModel> Get(string placa)
        {
            var carro = new Veiculo();

            var query =
                (from v in bd.Veiculos
                 where (v.placa == placa)
                 select new VerificaModel { placa = v.placa, marca = v.marca, modelo = v.modelo, cor = v.cor }).SingleOrDefault();

            return Ok(query);
        }

        [HttpPut("Adiciona/{placa}")]

        public ActionResult<string> Put([FromBody] AdicionaModel request)
        {
            var placa = request.placa;

            var veiculo = new Veiculo();


            var query =
                (from v in bd.Veiculos
                 where (v.placa == placa)
                 select v.placa).Any();

            if (query == true)
            {
                return BadRequest("Veiculo ja existente!");
            }

            veiculo.placa = request.placa;
            veiculo.marca = request.marca;
            veiculo.modelo = request.modelo;
            veiculo.cor = request.cor;

            bd.Veiculos.Add(veiculo);
            bd.SaveChanges();

            return Ok("Veiculo Adicionado!");
        }

        [HttpPut("DaEntrada/{Placa}")]

        public ActionResult<string> Put(string placa)
        {
            var patio = new Patio();

            var verificaAtivo =
                (from p in bd.Patios
                 where p.veiculoPlaca == placa && p.dataFim == null
                 select p.dataFim).Any();
            //.LastOrDefault();

            if(verificaAtivo)
                return BadRequest("Veiculo  ja esta no patio!");

            var dataAtual = DateTime.Now;

            patio.dataInicio = dataAtual;
            patio.veiculoPlaca = placa;

            bd.Add(patio);
            bd.SaveChanges();

            return Ok("Veiculo adicionado ao Patio!");

        }
    }
}

