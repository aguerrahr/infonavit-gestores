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
    public class EmpleadosController : ControllerBase
    {
        private readonly GestoresAPIContext context;
        private readonly ILogger<EmpleadosController> _logger;

        public EmpleadosController(GestoresAPIContext context, ILogger<EmpleadosController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult Query([FromQuery(Name = "in")] string identifier, [FromQuery] string type)
        {
            _logger.LogInformation("Query for in: " + identifier + " and type: " + type);
            
            var job = this.context.Jobs.FirstOrDefault(f => f.Name.Equals(type.ToUpper()));
            if(job == null)
            {
                return NoContent();
            }
            if (identifier != null && identifier.Length > 0)
            {
                var employee = this.context.Employees
                    .FirstOrDefault(e => e.ID.Equals(identifier) && e.IdJob.Equals(job.ID) && e.Enabled);
                return employee != null ?
                    new JsonResult(new EmpleadoDTO(employee))
                    : NoContent();
            }
            else {
                var employees = this.context.Employees.Where(e => e.IdJob.Equals(job.ID) && e.Enabled).ToList();
                return employees.Count > 0 ? 
                    new JsonResult(employees.Select(e => new EmpleadoDTO(e)))
                    : NoContent();
            }
        }

        [HttpGet("{in}")]
        [Produces("application/json")]
        public IActionResult Get([FromRoute(Name = "in")] string identifier)
        {
            _logger.LogInformation("Fetching employee informtion (eg: identidad) for employee id: " + identifier);
            if (identifier == null || identifier.Length == 0)
            {
                return BadRequest();
            }
            var employee = this.context.Employees.FirstOrDefault(e => e.IN.Equals(identifier) && e.Enabled);
            _logger.LogInformation("[Job:" + employee.Job.Name + "]");
            //_logger.LogInformation("[Sub Job:" + employee.Job.SubJobs.FirstOrDefault().Name + "]");
            //_logger.LogInformation("[Role: " + employee.Roles.FirstOrDefault().Name + "]");
            return new JsonResult(new EmpleadoDTO(employee));
        }

        [HttpPatch("{in}")]
        [Consumes("application/json")]
        public IActionResult Update([FromRoute(Name = "in")] string identifier, [FromBody] EmpleadoDTO employeeRequest)
        {
            _logger.LogInformation("Updting employee informtion, in: " + identifier);
            var employeeStored = this.context.Employees.FirstOrDefault(e => e.IN.Equals(identifier) && e.Enabled);
            if(employeeStored == null)
            {
                return NotFound();
            }
            
            employeeStored.IN = employeeRequest.IN;
            employeeStored.Name = employeeRequest.Name;
            employeeStored.MiddleName = employeeRequest.MiddleName;
            employeeStored.LastName = employeeRequest.LastName;
            employeeStored.CURP = employeeRequest.CURP;
            employeeStored.RFC = employeeRequest.RFC;
            employeeStored.NSS = employeeRequest.NSS;
            employeeStored.IdJob = employeeRequest.IdJob;
            employeeStored.Enrolled = employeeRequest.Enrolled;


            this.context.SaveChangesAsync();
            return Ok();
        }
    }
}
