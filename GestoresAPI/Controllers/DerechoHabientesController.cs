using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;

using GestoresAPI.DTO;
using GestoresAPI.Models.Contexts;

namespace GestoresAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DerechoHabientesController : ControllerBase
    {
        private readonly GestoresAPIContext context;
        private readonly ILogger<DerechoHabientesController> _logger;

        public DerechoHabientesController(GestoresAPIContext context, ILogger<DerechoHabientesController> logger)
        {
            this.context = context;
            _logger = logger;
        }
        [HttpGet("{id}")]
        [Produces("application/json")]
        public IActionResult Get([FromRoute(Name = "id")] int identifier)
        {
            _logger.LogInformation("Fetching derechohabiente informtion (eg: identidad) for derechohabiente id: " + identifier);
            if (identifier == null || identifier == 0)
            {
                return BadRequest();
            }
            //var dh = this.context.DerechoHabientes.FirstOrDefault(e => e.Id.Equals(identifier));

            var query = (
                from dh in context.DerechoHabientes.Where(a => a.Id == identifier)
                join tpdh in context.TipoDerechoHabientes
                on dh.TipoDerechoHabiente equals tpdh.Id
                select new DerechoHabienteDTO {
                    Id = dh.Id,
                    Nombre = dh.Nombre,
                    APaterno = dh.APaterno,
                    AMaterno = dh.AMaterno,
                    Nss = dh.Nss,
                    Curp = dh.Curp,
                    FhEnrolamiento = dh.FhEnrolamiento,
                    FhModificacion = dh.FhModificacion,
                    UsuarioEnrola = dh.UsuarioEnrola,
                    UsuarioModifica = dh.UsuarioModifica,
                    Activo = dh.Activo,
                    TipoDerechoHabiente = dh.TipoDerechoHabiente,
                    NbTpDH = tpdh.Descripcion
                }

            );
            var rows = query.ToList();
            return new JsonResult(rows);
        }
    }
}
