using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;

using GestoresAPI.DTO;
using GestoresAPI.Models;
using GestoresAPI.Models.Contexts;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestoresAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GestoresController: ControllerBase
    {
        private readonly ILogger<GestoresController> _logger;
        private readonly GestoresAPIContext context;

        public GestoresController(GestoresAPIContext context, ILogger<GestoresController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult QueryAll()
        {
            _logger.LogInformation("Query All Gestores");
            var job = this.context.Jobs.FirstOrDefault(f => f.Name.Equals(JobNamesEnum.GESTOR.ToString()));
            if (job == null)
            {
                _logger.LogError("El Job GESTOR no ha sido dado de alta en la BDs.");
                return NoContent();
            }
            /*
            var employees = this.context.Employees
                .Where(e => 
                    e.IdJob.Equals(job.ID) &&
                    e.Enabled)
                .ToList();
            return employees.Count > 0 ?
                new JsonResult(employees.Where(e => e.Enabled).Select(e => new EmpleadoDTO(e)))
                : NoContent();
            */
            IQueryable<EmpleadoDTO> employees =
                       (
                        from em in context.Employees
                        .Where(e => e.IdJob.Equals(job.ID) && e.Enabled)
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
                                    ).FirstOrDefault().ToString()
                        }
                       );            
            return employees.Count() > 0 ?
                    new JsonResult(employees)
                    : NoContent();
        }

        [HttpGet("empty")]
        [Produces("application/json")]
        public IActionResult QueryWithoutGerentes()
        {
            _logger.LogInformation("Query All Gestores without Gerente");
            var job = this.context.Jobs.FirstOrDefault(f => f.Name.Equals(JobNamesEnum.GESTOR.ToString()));
            if (job == null)
            {
                _logger.LogError("El Job GESTOR no ha sido dado de alta en la BDs.");
                return NoContent();
            }
            /*
            var employees = this.context.Employees
                .Where(e =>
                    e.IdJob.Equals(job.ID) &&
                    e.Enabled &&
                    e.SubEmployees.Count() == 0)
                .ToList();
            return employees.Count > 0 ?
                new JsonResult(
                    employees
                        .Select(e => new EmpleadoDTO(e)))
                : NoContent();
            */
            IQueryable<EmpleadoDTO> employees =
                       (
                        from em in context.Employees
                        .Where(e =>
                            e.IdJob.Equals(job.ID) &&
                            e.Enabled &&
                            e.SubEmployees.Count() == 0)
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
                                    ).FirstOrDefault().ToString()
                        }
                       );
            return employees.Count() > 0 ?
                new JsonResult(employees)
                : NoContent();
        }

        [HttpGet("{in}")]
        [Produces("application/json")]
        public IActionResult Get([FromRoute(Name = "in")] string identifier)
        {
            _logger.LogInformation("Fetching gestor for IN: " + identifier);
            if (identifier == null || identifier.Length == 0)
            {
                return BadRequest();
            }
            /*
            var employee = this.context.Employees
                .FirstOrDefault(e => 
                    e.Job.Name.Equals(JobNamesEnum.GESTOR.ToString()) && 
                    e.IN.Equals(identifier) &&
                    e.Enabled);
            return employee != null
                ? new JsonResult(new EmpleadoDTO(employee))
                : NoContent();
            */
            IQueryable<EmpleadoDTO> employee =
                       (
                        from em in context.Employees.Where(e => e.Job.Name.Equals(JobNamesEnum.GESTOR.ToString()) && e.IN.Equals(identifier) && e.Enabled)
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
                                    ).FirstOrDefault().ToString()
                        }
                       ).Take(1);
            return employee != null
                ? new JsonResult(employee)
                : NoContent();
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Add([FromBody] EmpleadoDTO employeeRequest)
        {
            _logger.LogInformation("Adding gestor for IN: " + employeeRequest.IN);
            //Request Validations
            if (employeeRequest.IN.Length > 15)
            {
                return BadRequest("Field IN is too long.");
            }
            if (employeeRequest.Name.Length > 50)
            {
                return BadRequest("Field Name is too long.");
            }
            if (employeeRequest.LastName.Length > 50)
            {
                return BadRequest("Field LastName is too long.");
            }
            if (employeeRequest.MiddleName.Length > 50)
            {
                return BadRequest("Field MiddleName is too long.");
            }
            if (employeeRequest.CURP.Length > 18)
            {
                return BadRequest("Field CURP is too long.");
            }
            if (employeeRequest.RFC.Length > 13)
            {
                return BadRequest("Field RFC is too long.");
            }
            if (employeeRequest.NSS.Length > 11)
            {
                return BadRequest("Field NSS is too long.");
            }
            //Validation if employee exist
            var verifyEmployee = this.context.Employees
                .FirstOrDefault(e => 
                    e.IN.Equals(employeeRequest.IN));
            if (null != verifyEmployee && verifyEmployee.Enabled)
            {
                return Conflict("El gestor ya se encuentra reistrado.");
            }
            if (null != verifyEmployee && !verifyEmployee.Enabled)
            {
                verifyEmployee.ID           = employeeRequest.IN;
                verifyEmployee.IN           = employeeRequest.IN;
                verifyEmployee.Name         = employeeRequest.Name;
                verifyEmployee.MiddleName   = employeeRequest.MiddleName;
                verifyEmployee.LastName     = employeeRequest.LastName;
                verifyEmployee.CURP         = employeeRequest.CURP;
                verifyEmployee.RFC          = employeeRequest.RFC;
                verifyEmployee.NSS          = employeeRequest.NSS;
                verifyEmployee.Enabled      = true;
                verifyEmployee.UpdatedAt    = DateTime.Now;
                verifyEmployee.InRegistra   = null;
                verifyEmployee.InModifica   = employeeRequest.InModifica;
                /*------------------------------------------- History --------------------------------------------*/
                    var employeehistory = new EmployeesHistory()
                    {
                        ID          = employeeRequest.IN,
                        IN          = employeeRequest.IN,
                        Name        = employeeRequest.Name,
                        MiddleName  = employeeRequest.MiddleName,
                        LastName    = employeeRequest.LastName,
                        CURP        = employeeRequest.CURP,
                        RFC         = employeeRequest.RFC,
                        NSS         = employeeRequest.NSS,
                        Enabled     = true,
                        IdJob       = employeeRequest.IdJob,
                        CreatedAt   = DateTime.Now, //zutjmx@gmail.com 20/01/2023
                        InRegistra  = employeeRequest.InModifica,
                        FacultyId   = 5 //Actualizar
                    };
                    this.context.EmployeesHistories.Add(employeehistory);
                /*------------------------------------------------------------------------------------------------*/
                this.context.SaveChanges();
                return new CreatedResult(
                new Uri("/Gerentes/" + verifyEmployee.IN, UriKind.Relative),
                new EmpleadoDTO(verifyEmployee));
            }
            else
            {
                var job = this.context.Jobs.FirstOrDefault(f => f.Name.Equals(JobNamesEnum.GESTOR.ToString()));
                var rolle = this.context.Roles.FirstOrDefault(a => a.Name.Contains(JobNamesEnum.GESTOR.ToString()));
                if (job == null)
                {
                    _logger.LogError("El Job GESTOR no ha sido dado de alta en la BDs.");
                    return NoContent();
                }
                if (rolle == null)
                {
                    _logger.LogError("El Rol GESTOR no ha sido dado de alta en la BDs.");
                    return NoContent();
                }
                var employee = new Employee()
                {
                    ID          = employeeRequest.IN,
                    IN          = employeeRequest.IN,
                    Name        = employeeRequest.Name,
                    MiddleName  = employeeRequest.MiddleName,
                    LastName    = employeeRequest.LastName,
                    CURP        = employeeRequest.CURP,
                    RFC         = employeeRequest.RFC,
                    NSS         = employeeRequest.NSS,
                    Enabled     = true,
                    IdJob       = job.ID,
                    CreatedAt   = DateTime.Now,
                    Roles       = new List<Role>() { rolle },
                    InRegistra  = employeeRequest.InRegistra,
                    InModifica  = null
                };
                this.context.Employees.Add(employee);
                /*------------------------------------------- History --------------------------------------------*/
                    var employeehistory = new EmployeesHistory
                    {
                        ID          = employeeRequest.IN,
                        IN          = employeeRequest.IN,
                        Name        = employeeRequest.Name,
                        MiddleName  = employeeRequest.MiddleName,
                        LastName    = employeeRequest.LastName,
                        CURP        = employeeRequest.CURP,
                        RFC         = employeeRequest.RFC,
                        NSS         = employeeRequest.NSS,
                        Enabled     = true,
                        IdJob       = employeeRequest.IdJob,
                        CreatedAt   = DateTime.Now,
                        InRegistra  = employeeRequest.InRegistra,
                        InModifica  = null,
                        FacultyId   = 1 //Alta
                    };                
                this.context.EmployeesHistories.Add(employeehistory);
                /*------------------------------------------------------------------------------------------------*/
                this.context.SaveChanges();
                return new CreatedResult(
                    new Uri("/Gestores/" + employee.IN, UriKind.Relative),
                    new EmpleadoDTO(employee));
            }
        }

        [HttpDelete("{in}")]
        public IActionResult Delete([FromRoute(Name = "in")] string identifier)
        {
            if (identifier == null || identifier.Length == 0)
            {
                return BadRequest();
            }
            _logger.LogInformation("Removing gestor for IN: " + identifier);
            var employee = this.context.Employees
                .FirstOrDefault(e => e.IN.Equals(identifier) && e.Enabled);
            if (employee == null)
            {
                return NotFound();
            }
            employee.Enabled = false;
            employee.InModifica = identifier;
            //this.context.Employees.Remove(employee);
            /*------------------------------------------- History --------------------------------------------*/
                var employeehistory = new EmployeesHistory()
                {
                    ID          = employee.IN,
                    IN          = employee.IN,
                    Name        = employee.Name,
                    MiddleName  = employee.MiddleName,
                    LastName    = employee.LastName,
                    CURP        = employee.CURP,
                    RFC         = employee.RFC,
                    NSS         = employee.NSS,
                    Enabled     = true,
                    IdJob       = employee.IdJob,
                    UpdatedAt   = DateTime.Now,
                    //InRegistra  = employee.InRegistra,
                    InModifica  = employee.InModifica,
                    FacultyId   = 2 //Baja
                };
                this.context.EmployeesHistories.Add(employeehistory);
            /*------------------------------------------------------------------------------------------------*/
            this.context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{in}/gerentes")]
        [Consumes("application/json")]
        public IActionResult AddGernte([FromRoute(Name = "in")] string identifier, [FromBody] EmpleadoDTO[] gerentesRequest)
        {
            _logger.LogInformation("Adding " + gerentesRequest.Length + " Gerentes to Gestor: " + identifier);
            var employee = this.context.Employees
                .FirstOrDefault(e =>
                    e.Job.Name.Equals(JobNamesEnum.GESTOR.ToString()) &&
                    e.IN.Equals(identifier) &&
                    e.Enabled);
            if (employee == null)
            {
                return NotFound(ErrorType.GestorNotFound.GetString());
            }
            var gerentesToAdd = new List<Employee>();
            foreach (EmpleadoDTO gerenteRequest in gerentesRequest)
            {
                var gerente = this.context.Employees
                .FirstOrDefault(e =>
                    e.Job.Name.Equals(JobNamesEnum.GERENTE.ToString()) &&
                    e.ID.Equals(gerenteRequest.IN) &&
                    e.Enabled);
                if (gerente == null)
                {
                    return NotFound(ErrorType.GerenteNotFound.GetString());
                }
                gerentesToAdd.Add(gerente);
            }
            foreach (Employee gerenteToAdd in gerentesToAdd)
            {
                var gerenteAdded = employee.SubEmployees.FirstOrDefault(se => se.IN.Equals(gerenteToAdd.IN));
                if (gerenteAdded == null)
                {
                    employee.SubEmployees.Add(gerenteToAdd);
                    this.context.SaveChanges();
                }
            }
            /*------------------------------------------- History --------------------------------------------*/
                var employeehistory = new EmployeesHistory()
                {
                    ID          = employee.IN,
                    IN          = employee.IN,
                    Name        = employee.Name,
                    MiddleName  = employee.MiddleName,
                    LastName    = employee.LastName,
                    CURP        = employee.CURP,
                    RFC         = employee.RFC,
                    NSS         = employee.NSS,
                    Enabled     = true,
                    IdJob       = employee.IdJob,
                    CreatedAt   = DateTime.Now,
                    InRegistra  = employee.InRegistra,
                    //InModifica  = employee.InModifica,
                    FacultyId   = 1 //Alta
                };
                this.context.EmployeesHistories.Add(employeehistory);
                this.context.SaveChanges();
            /*------------------------------------------------------------------------------------------------*/
            return Ok();
        }

        [HttpGet("{in}/gerentes")]
        [Consumes("application/json")]
        public IActionResult GetAllGerentes([FromRoute(Name = "in")] string identifier)
        {
            _logger.LogInformation("Fetching Gerentes of Gestor: " + identifier);

            var employee = this.context.Employees
                .FirstOrDefault(e =>
                    e.Job.Name.Equals(JobNamesEnum.GESTOR.ToString()) &&
                    e.IN.Equals(identifier) &&
                    e.Enabled);
            if (employee == null)
            {
                return NotFound(ErrorType.GestorNotFound.GetString());
            }
            var gerentes = employee.SubEmployees;
            return gerentes.Count > 0 ?
                new JsonResult(gerentes.Select(e => new EmpleadoDTO(e)))
                : NoContent();
        }

    }
}
