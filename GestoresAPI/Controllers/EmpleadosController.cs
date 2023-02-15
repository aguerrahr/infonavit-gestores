using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using GestoresAPI.DTO;
using GestoresAPI.Models;
using GestoresAPI.Models.Contexts;
using System;

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
        [HttpGet("EmployeesEntity")]
        [Produces("application/json")]
        public IActionResult EmployeesEntity()
        {
            var employee =
                        (from em in context.Employees
                         select new
                         {
                             IN = em.IN,
                             Name = em.Name,
                             LastName = em.LastName,
                             MiddleName = em.MiddleName,
                             CURP = em.CURP,
                             RFC = em.RFC,
                             NSS = em.NSS,
                             IdJob = em.IdJob,
                             Enrolled = em.Enrolled,
                             InRegistra = em.InRegistra,
                             InModifica = em.InModifica,
                             NbRegistra = (em.InRegistra == null
                             ? ""
                             : ((from b in context.Employees
                                 where b.IN == em.InRegistra
                                 select (b.Name + " " + b.LastName + " " + b.MiddleName)).FirstOrDefault().ToString())
                             ),
                             NbModifica =
                             em.InModifica == null
                             ? ""
                             : (
                                     from u in context.Employees
                                     where (u.IN == em.InModifica)
                                     select (u.Name + " " + u.LastName + " " + u.MiddleName)
                                 ).FirstOrDefault().ToString(),
                             Enabled = em.Enabled
                         });
            return employee != null ?
                new JsonResult(employee)
                : NoContent();
        }
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Query([FromQuery(Name = "in")] string identifier, [FromQuery] string type)
        {
            _logger.LogInformation("Query for in: " + identifier + " and type: " + type);

            var job = this.context.Jobs.FirstOrDefault(f => f.Name.Equals(type.ToUpper()));
            if (job == null)
            {
                return NoContent();
            }
            if (identifier != null && identifier.Length > 0)
            {
                //var employee = this.context.Employees
                //    .FirstOrDefault(e => e.ID.Equals(identifier) && e.IdJob.Equals(job.ID) && e.Enabled);
                IQueryable<EmpleadoDTO> employee =
                           (
                           from em in context.Employees.Where(e => e.ID.Equals(identifier) && e.IdJob.Equals(job.ID) && e.Enabled)
                           select new EmpleadoDTO
                           {
                               IN = em.IN,
                               Name = em.Name,
                               LastName = em.LastName,
                               MiddleName = em.MiddleName,
                               CURP = em.CURP,
                               RFC = em.RFC,
                               NSS = em.NSS,
                               IdJob = em.IdJob,
                               Enrolled = em.Enrolled,
                               InRegistra = em.InRegistra,
                               InModifica = em.InModifica,
                               NbRegistra = (em.InRegistra == null
                               ? ""
                               : ((from b in context.Employees
                                   where b.IN == em.InRegistra
                                   select (b.Name + " " + b.LastName + " " + b.MiddleName)).FirstOrDefault().ToString())
                               ),
                               NbModifica =
                               em.InModifica == null
                               ? ""
                               : (
                                        from u in context.Employees
                                        where (u.IN == em.InModifica)
                                        select (u.Name + " " + u.LastName + " " + u.MiddleName)
                                    ).FirstOrDefault().ToString(),
                           }
                           );
                return employee != null ?
                    new JsonResult(employee)
                    : NoContent();
            }
            else
            {
                //var employees = this.context.Employees.Where(e => e.IdJob.Equals(job.ID) && e.Enabled).ToList();
                IQueryable<EmpleadoDTO> employees =
                           (
                           from em in context.Employees.Where(e => e.IdJob.Equals(job.ID) && e.Enabled)
                           select new EmpleadoDTO
                           {
                               IN = em.IN,
                               Name = em.Name,
                               LastName = em.LastName,
                               MiddleName = em.MiddleName,
                               CURP = em.CURP,
                               RFC = em.RFC,
                               NSS = em.NSS,
                               IdJob = em.IdJob,
                               Enrolled = em.Enrolled,
                               InRegistra = em.InRegistra,
                               InModifica = em.InModifica,
                               NbRegistra = (em.InRegistra == null
                               ? ""
                               : ((from b in context.Employees
                                   where b.IN == em.InRegistra
                                   select (b.Name + " " + b.LastName + " " + b.MiddleName)).FirstOrDefault().ToString())
                               ),
                               NbModifica =
                               em.InModifica == null
                               ? ""
                               : (
                                        from u in context.Employees
                                        where (u.IN == em.InModifica)
                                        select (u.Name + " " + u.LastName + " " + u.MiddleName)
                                    ).FirstOrDefault().ToString(),
                           }
                           );
                return employees.Count() > 0 ?
                    new JsonResult(employees)
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
            //var employee = this.context.Employees.FirstOrDefault(e => e.IN.Equals(identifier) && e.Enabled);
            IQueryable<EmpleadoDTO> employee =
                            (
                            from em in context.Employees.Where(e => e.IN.Equals(identifier) && e.Enabled)
                            select new EmpleadoDTO
                            {
                                IN = em.IN,
                                Name = em.Name,
                                LastName = em.LastName,
                                MiddleName = em.MiddleName,
                                CURP = em.CURP,
                                RFC = em.RFC,
                                NSS = em.NSS,
                                IdJob = em.IdJob,
                                Enrolled = em.Enrolled,
                                InRegistra = em.InRegistra,
                                InModifica = em.InModifica,
                                NbRegistra = (em.InRegistra == null
                               ? ""
                               : ((from b in context.Employees
                                   where b.IN == em.InRegistra
                                   select (b.Name + " " + b.LastName + " " + b.MiddleName)).FirstOrDefault().ToString())
                               ),
                                NbModifica =
                               em.InModifica == null
                               ? ""
                               : (
                                        from u in context.Employees
                                        where (u.IN == em.InModifica)
                                        select (u.Name + " " + u.LastName + " " + u.MiddleName)
                                    ).FirstOrDefault().ToString(),
                            }
                            );
            //_logger.LogInformation("[Job:" + employee.Job.Name + "]");
            //_logger.LogInformation("[Sub Job:" + employee.Job.SubJobs.FirstOrDefault().Name + "]");
            //_logger.LogInformation("[Role: " + employee.Roles.FirstOrDefault().Name + "]");
            return new JsonResult(employee);
        }

        [HttpPatch("{in}")]
        [Consumes("application/json")]
        public IActionResult Update([FromRoute(Name = "in")] string identifier, [FromBody] EmpleadoDTO employeeRequest)
        {
            _logger.LogInformation("Updting employee informtion, in: " + identifier);
            var employeeStored = this.context.Employees.FirstOrDefault(e => e.IN.Equals(identifier) && e.Enabled);
            if (employeeStored == null)
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
            employeeStored.Enrolled     = 1; //zutjmx@gmail.com 14/02/2023 La regué gacho, ahora siempre se va como 1
            //employeeStored.UpdatedAt    = employeeRequest.UpdatedAt;
            employeeStored.UpdatedAt = DateTime.Now; //zutjmx@gmail.com 26/01/2023 La fecha de actualización se iba vacía
            //employeeStored.InRegistra = employeeRequest.InRegistra;
            employeeStored.InModifica = employeeRequest.InModifica;
            employeeStored.Enabled = employeeRequest.Enabled;
            this.context.SaveChanges();
            /*------------------------------------------- History --------------------------------------------*/
            var employeehistory = new EmployeesHistory()
            {
                ID = employeeRequest.IN,
                IN = employeeRequest.IN,
                Name = employeeRequest.Name,
                MiddleName = employeeRequest.MiddleName,
                LastName = employeeRequest.LastName,
                CURP = employeeRequest.CURP,
                RFC = employeeRequest.RFC,
                NSS = employeeRequest.NSS,
                Enabled = true,
                IdJob = employeeRequest.IdJob,
                CreatedAt = DateTime.Now, //zutjmx@gmail.com 20/01/2023
                InRegistra = employeeRequest.InModifica,
                FacultyId = 5 //Actualizar
            };
            this.context.EmployeesHistories.Add(employeehistory);
            /*------------------------------------------------------------------------------------------------*/
            this.context.SaveChanges();
            return Ok();
        }

        [HttpPatch()]
        [Consumes("application/json")]
        public IActionResult EnableDisableEmployee([FromBody] EmpleadoDTO employeeRequest)
        {
            _logger.LogInformation("Updting employee informtion, in: " + employeeRequest.IN);
            var employeeStored = this.context.Employees.FirstOrDefault(e => e.IN.Equals(employeeRequest.IN));
            if (employeeStored == null)
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
            employeeStored.UpdatedAt = DateTime.Now; //zutjmx@gmail.com 26/01/2023 La fecha de actualización se iba vacía
            
            employeeStored.InModifica = employeeRequest.InModifica;
            employeeStored.Enabled = employeeRequest.Enabled;
            this.context.SaveChanges();
            /*------------------------------------------- History --------------------------------------------*/
            var employeehistory = new EmployeesHistory()
            {
                ID = employeeRequest.IN,
                IN = employeeRequest.IN,
                Name = employeeRequest.Name,
                MiddleName = employeeRequest.MiddleName,
                LastName = employeeRequest.LastName,
                CURP = employeeRequest.CURP,
                RFC = employeeRequest.RFC,
                NSS = employeeRequest.NSS,
                Enabled = true,
                IdJob = employeeRequest.IdJob,
                CreatedAt = DateTime.Now, //zutjmx@gmail.com 20/01/2023
                InRegistra = employeeRequest.InModifica,
                FacultyId = (employeeRequest.Enabled == true ? 6 : 7), //Actualizar
            };
            this.context.EmployeesHistories.Add(employeehistory);
            /*------------------------------------------------------------------------------------------------*/
            this.context.SaveChanges();
            return Ok();
        }

    }
}
