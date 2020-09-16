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
    [Route("api/veiculo")]
    [ApiController]
    public class VeiculoController : ControllerBase
    {
        EstacionamentoContext bd = new EstacionamentoContext();

        [HttpGet("verificaPlaca/{placa}")]
        public ActionResult<VerificaModel> VerificaPlaca(string placa)
        {
            var carro = new Veiculo();

            var query =
                (from v in bd.Veiculos
                 where (v.placa == placa)
                 select new VerificaModel { placa = v.placa, marca = v.marca, modelo = v.modelo, cor = v.cor }).SingleOrDefault();

            return Ok(query);
        }

        [HttpPut("adiciona/{placa}")]
        public ActionResult<string> Adiciona([FromBody] AdicionaModel request)
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

        [HttpGet("ativos")]
        public List<AtivosModel> Ativos()
        {
            var query =
                (from p in bd.Patios
                 join v in bd.Veiculos on p.veiculoPlaca equals v.placa
                 where p.dataFim == null
                 select new AtivosModel { placa = v.placa, marca = v.marca, modelo = v.modelo, cor = v.cor, entrada = p.dataInicio }).ToList();

            return query;
        }
    }
}
