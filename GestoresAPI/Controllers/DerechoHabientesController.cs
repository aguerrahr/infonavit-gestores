using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;

using GestoresAPI.DTO;
using GestoresAPI.Models.Contexts;
using Castle.DynamicProxy.Generators;

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
                    NbTpDH = tpdh.Descripcion,
                    InUsuairoEnrola= dh.UsuarioEnrola,
                    InUsuairoModifica = dh.UsuarioModifica,
                    NbUsuairoEnrola = (
                        from u in context.Employees
                        where (u.IN == dh.UsuarioEnrola)
                        select (u.Name + " " + u.LastName + " " + u.MiddleName)
                    ).FirstOrDefault().ToString(),
                    NbUsuairoModifica =
                    dh.UsuarioModifica == null
                    ? ""
                    : (
                            from u in context.Employees
                            where (u.IN == dh.UsuarioModifica)
                            select (u.Name + " " + u.LastName + " " + u.MiddleName)
                        ).FirstOrDefault().ToString(),
                }

            );
            var rows = query.ToList();
            return new JsonResult(rows);
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Fetching derechohabiente vs employee informtion (eg: identidad) for derechohabiente ");                       
            var query = (
                from dh in context.DerechoHabientes
                join tpdh in context.TipoDerechoHabientes
                on dh.TipoDerechoHabiente equals tpdh.Id
                select new DerechoHabienteDTO
                {
                    Id = dh.Id,
                    Nombre = dh.Nombre,
                    APaterno = dh.APaterno,
                    AMaterno = dh.AMaterno,
                    Nss = dh.Nss,
                    Curp = dh.Curp,
                    FhEnrolamiento = dh.FhEnrolamiento,
                    FhModificacion = dh.FhModificacion,
                    UsuarioEnrola = dh.UsuarioEnrola,
                    UsuarioModifica =  dh.UsuarioModifica == null ? "" : dh.UsuarioModifica,
                    Activo = dh.Activo,
                    TipoDerechoHabiente = dh.TipoDerechoHabiente,
                    NbTpDH = tpdh.Descripcion,
                    InUsuairoEnrola = dh.UsuarioEnrola,
                    InUsuairoModifica = dh.UsuarioModifica,
                    NbUsuairoEnrola = dh.UsuarioEnrola == "MOD-ENROLL" ? "MODULO DE ENROLAMIENTO" : (
                        from u in context.Employees
                        where (u.IN == dh.UsuarioEnrola)
                        select (u.Name + " " + u.LastName + " " + u.MiddleName)
                    ).FirstOrDefault().ToString(),
                    NbUsuairoModifica =
                    dh.UsuarioModifica == null
                    ? ""
                    : (
                            from u in context.Employees
                            where (u.IN == dh.UsuarioModifica)
                            select (u.Name + " " + u.LastName + " " + u.MiddleName)
                        ).FirstOrDefault().ToString(),
                }

            );
            var rows = query.ToList();
            return new JsonResult(rows);
        }
        [HttpGet("GetTpDh")]
        [Produces("application/json")]
        public IActionResult GetTpDh()
        {
            _logger.LogInformation("Fetching Tipo derechohabiente");
            var query = (
                from tdh in context.TipoDerechoHabientes
                select new {
                    Id = tdh.Id,
                    Descripcion = tdh.Descripcion
                }
            );
            var rows= query.ToList();
            return new JsonResult(rows);
        }
    }
}
