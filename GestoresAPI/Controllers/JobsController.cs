using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Linq;

using GestoresAPI.DTO;
using GestoresAPI.Models.Contexts;
using Castle.DynamicProxy.Generators;
using System.Xml.Linq;

namespace GestoresAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {

        private readonly GestoresAPIContext context;
        private readonly ILogger<JobsController> _logger;
        public JobsController(GestoresAPIContext context, ILogger<JobsController> logger)
        {
            this.context = context;
            _logger = logger;
        }
        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Fetching derechohabiente vs employee informtion (eg: identidad) for derechohabiente ");
            var query = (
                from j in context.Jobs                                
                select new JobDTO
                {
                    ID = j.ID, 
                    Name = j.Name, 
                    Description = j.Description, 
                    CreatedAt = j.CreatedAt,
                    Enabled = j.Enabled
                }
            );
            var rows = query.ToList();

            //zutjmx@gmail.com: que aparezca ASESOR en lugar de GESTOR ini
            foreach (var row in rows)
            {
                if(row.Name.Equals("GESTOR",System.StringComparison.OrdinalIgnoreCase))
                {
                    row.Name = "ASESOR";
                }

                if (row.Name.Equals("ADMINISTRATIVO", System.StringComparison.OrdinalIgnoreCase))
                {
                    row.Name = "ADMINISTRADOR";
                }
            }
            //zutjmx@gmail.com: que aparezca ASESOR en lugar de GESTOR fin

            return new JsonResult(rows);
        }
    }
}
