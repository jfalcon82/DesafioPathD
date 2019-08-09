using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetoSpsaPathD.Models;

namespace RetoSpsaPathD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ClienteController(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Endpoint de salida GET /listclientes 
        /// Lista de personas con todos los datos + fecha probable de muerte de cada una
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [HttpGet("listclientes")]
        public IEnumerable<Cliente> GetAll()
        {
            return context.Clientes.ToList();
        }
        /// <summary>
        /// Endpoint de salida GET  /kpideclientes
        ///Promedio edad entre todos los clientes
        ///Desviación estándar entre las edades de todos los clientes
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [HttpGet("kpideclientes")]
        public IEnumerable<KpiCliente> GetKpi()
        {
            List<Cliente> Clientes = context.Clientes.ToList();
            KpiCliente Kpi = new KpiCliente();

            var promedioEdad = (from cliente in Clientes select cliente.Edad).Average();
            var desviacionEstandar = CarlulaDesvEstandar(Clientes.Select(x => (double)x.Edad));
            Kpi.PromedioEdad = promedioEdad;
            Kpi.DesviacionEstandar = desviacionEstandar;
            Kpi.TotalClientes = Clientes.Count();

            yield return Kpi;            
        }

        private double CarlulaDesvEstandar(IEnumerable<double> valores)
        {
            double ret = 0;
            if (valores.Count() > 0)
            {               
                double avg = valores.Average();               
                double sum = valores.Sum(d => Math.Pow(d - avg, 2));                 
                ret = Math.Sqrt((sum) / (valores.Count() - 1));
            }
            return ret;
        }

        /// <summary>
        /// Endpoint de Entrada POST /creacliente (formato de fecha "YYYY-MM-DD")
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("creacliente")]
        public IActionResult CreateCliente([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var RandomDiasVida = new Random().Next(25000, 37000);
            cliente.RandomDiasVida = RandomDiasVida;
           

            context.Clientes.Add(cliente);
            context.SaveChanges();

            var clienteNuevo = context.Clientes.FirstOrDefault(x => x.Id == cliente.Id);
         
            return Created("", cliente);
      
        }    

    }
}