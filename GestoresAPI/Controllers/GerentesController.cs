using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;

using GestoresAPI.DTO;
using GestoresAPI.Models;
using GestoresAPI.Models.Contexts;
using System.Collections.Generic;
using System.Data;


namespace GestoresAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GerentesController: ControllerBase
    {
        private readonly ILogger<GerentesController> _logger;
        private readonly GestoresAPIContext context;

        public GerentesController(GestoresAPIContext context, ILogger<GerentesController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult QueryAll()
        {
            _logger.LogInformation("Query All Gerentes");
            var job = this.context.Jobs.FirstOrDefault(f => f.Name.Equals(JobNamesEnum.GERENTE.ToString()));
            if (job == null)
            {
                _logger.LogError("El Job GERENTE no ha sido dado de alta en la BDs.");
                return NoContent();
            }
            /*
             var employees = this.context.Employees
                .Where(e => 
                    e.IdJob.Equals(job.ID) &&
                    e.Enabled)
                .ToList();
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
                           InRegistra = (em.InRegistra == null
                           ? ""
                           : ((from b in context.Employees
                               where b.IN == em.InRegistra
                               select (b.Name + " " + b.LastName + " " + b.MiddleName)).FirstOrDefault().ToString())
                           ),
                           InModifica =
                           em.InModifica == null
                           ? ""
                           : (
                                    from u in context.Employees
                                    where (u.IN == em.InModifica)
                                    select (u.Name + " " + u.LastName + " " + u.MiddleName)
                                ).FirstOrDefault().ToString()
                       }
                       );
            /*
            return employees.Count() > 0 ?
                    new JsonResult(employees.Where(e => e.Enabled).Select(e => new EmpleadoDTO(e)))
                    : NoContent();
            */
            return employees.Count() > 0 ?
                    new JsonResult(employees)
                    : NoContent();
        }

        [HttpGet("empty")]
        [Produces("application/json")]
        public IActionResult QueryWithoutGestores()
        {
            _logger.LogInformation("Query All Gerentes without Gestores");
            var job = this.context.Jobs.FirstOrDefault(f => f.Name.Equals(JobNamesEnum.GERENTE.ToString()));
            if (job == null)
            {
                _logger.LogError("El Job GERENTE no ha sido dado de alta en la BDs.");
                return NoContent();
            }
            /*
            var employees = this.context.Employees
                .Where(e =>
                    e.IdJob.Equals(job.ID) &&
                    e.Enabled &&
                    e.Managers.Count() == 0)
                .ToList();
            return employees.Count > 0 ?
                new JsonResult(
                    employees
                        .Select(e => new EmpleadoDTO(e)))
                : NoContent();
            */
            IQueryable<EmpleadoDTO> employees =
                       (
                        from em in context.Employees.Where(e => e.IdJob.Equals(job.ID) && e.Enabled && e.Managers.Count() == 0)
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
                            InRegistra = (em.InRegistra == null
                           ? ""
                           : ((from b in context.Employees
                               where b.IN == em.InRegistra
                               select (b.Name + " " + b.LastName + " " + b.MiddleName)).FirstOrDefault().ToString())
                           ),
                            InModifica =
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
            _logger.LogInformation("Fetching gerente for IN: " + identifier);
            if (identifier == null || identifier.Length == 0)
            {
                return BadRequest();
            }
            /*
            var employee = this.context.Employees
                .FirstOrDefault(e => 
                    e.Job.Name.Equals(JobNamesEnum.GERENTE.ToString()) && 
                    e.IN.Equals(identifier) &&
                    e.Enabled);
            return employee != null
                ? new JsonResult(new EmpleadoDTO(employee))
                : NoContent();   
            */
            IQueryable<EmpleadoDTO> employee =
                       (
                        from em in context.Employees.Where(e => e.Job.Name.Equals(JobNamesEnum.GERENTE.ToString()) && e.IN.Equals(identifier) && e.Enabled)
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
                            InRegistra = (em.InRegistra == null
                           ? ""
                           : ((from b in context.Employees
                               where b.IN == em.InRegistra
                               select (b.Name + " " + b.LastName + " " + b.MiddleName)).FirstOrDefault().ToString())
                           ),
                            InModifica =
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
            _logger.LogInformation("Adding gerente for IN: " + employeeRequest.IN);
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
                return Conflict("El gerente ya se encuentra reistrado.");
            }
            if (null != verifyEmployee && !verifyEmployee.Enabled)
            {
                verifyEmployee.ID = employeeRequest.IN;
                //verifyEmployee.ID = Guid.NewGuid().ToString();
                verifyEmployee.IN = employeeRequest.IN;
                verifyEmployee.Name = employeeRequest.Name;
                verifyEmployee.MiddleName = employeeRequest.MiddleName;
                verifyEmployee.LastName = employeeRequest.LastName;
                verifyEmployee.CURP = employeeRequest.CURP;
                verifyEmployee.RFC = employeeRequest.RFC;
                verifyEmployee.NSS = employeeRequest.NSS;
                verifyEmployee.Enabled = true;
                verifyEmployee.CreatedAt = DateTime.Now;
                verifyEmployee.InRegistra = employeeRequest.InRegistra;
                verifyEmployee.InModifica = employeeRequest.InModifica;

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
                        CreatedAt = DateTime.Now,
                        InRegistra = employeeRequest.InRegistra,
                        InModifica = employeeRequest.InModifica,
                        FacultyId = 5 //Actualizar
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
                var job = this.context.Jobs.FirstOrDefault(f => f.Name.Equals(JobNamesEnum.GERENTE.ToString()));
                var rolle = this.context.Roles.FirstOrDefault(a => a.Name.Contains(JobNamesEnum.GERENTE.ToString()));

                if (job == null)
                {
                    _logger.LogError("El Job GERENTE no ha sido dado de alta en la BDs.");
                    return NoContent();
                }
                if (rolle == null)
                {
                    _logger.LogError("El Rol GERENTE no ha sido dado de alta en la BDs.");
                    return NoContent();
                }
                var employee = new Employee()
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
                    IdJob = job.ID,
                    CreatedAt = DateTime.Now,                    
                    Roles = new List<Role>() { rolle },
                    InRegistra = employeeRequest.InRegistra,
                    InModifica = employeeRequest.InModifica
                };
                this.context.Employees.Add(employee);
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
                        CreatedAt = DateTime.Now,
                        InRegistra = employeeRequest.InRegistra,
                        InModifica = employeeRequest.InModifica,
                        FacultyId = 1 //Alta
                    };
                    this.context.EmployeesHistories.Add(employeehistory);
                /*------------------------------------------------------------------------------------------------*/
                this.context.SaveChanges();
                //Agragar el nuevo Role del empleado GERENTE
                return new CreatedResult(
                    new Uri("/Gerentes/" + employee.IN, UriKind.Relative),
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
            _logger.LogInformation("Removing gerente for IN: " + identifier);
            var employee = this.context.Employees
                .FirstOrDefault(e => e.IN.Equals(identifier) && e.Enabled);
            if (employee == null)
            {
                return NotFound();
            }
            employee.Enabled = false;
            employee.InRegistra = identifier;
            //this.context.Employees.Remove(employee);
            this.context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{in}/gestores")]
        [Consumes("application/json")]
        public IActionResult AddGestor([FromRoute(Name = "in")] string identifier, [FromBody] EmpleadoDTO[] gestoresRequest)
        {
            _logger.LogInformation("Adding " + gestoresRequest.Length + " Gestores to Gerente: " + identifier);
            var employee = this.context.Employees
                .FirstOrDefault(e => 
                    e.Job.Name.Equals(JobNamesEnum.GERENTE.ToString()) && 
                    e.IN.Equals(identifier) &&
                    e.Enabled);
            if (employee == null)
            {
                return NotFound(ErrorType.GerenteNotFound.GetString());
            }
            var gestoresToAdd = new List<Employee>();
            foreach (EmpleadoDTO gestorRequest in gestoresRequest) {
                var gestor = this.context.Employees
                    .FirstOrDefault(e =>
                        e.Job.Name.Equals(JobNamesEnum.GESTOR.ToString()) &&
                        e.IN.Equals(gestorRequest.IN) &&
                        e.Enabled);
                if (gestor == null)
                {
                    return NotFound(gestorRequest.IN + ": " + ErrorType.GestorNotFound.GetString());
                }
                gestoresToAdd.Add(gestor);
            }
            foreach (Employee gestorToAdd in gestoresToAdd)
            {
                var managerAdded = employee.Managers.FirstOrDefault(m => m.IN.Equals(gestorToAdd.IN));
                if (managerAdded == null)
                {
                    employee.Managers.Add(gestorToAdd);
                    this.context.SaveChanges();
                }
            }
            return Ok();
        }

        [HttpGet("{in}/gestores")]
        [Consumes("application/json")]
        public IActionResult GetAllGestores([FromRoute(Name ="in")] string identifier)
        {
            _logger.LogInformation("Fetching Gestores of Gerente: " + identifier);
            
            var employee = this.context.Employees
                .FirstOrDefault(e => 
                    e.Job.Name.Equals(JobNamesEnum.GERENTE.ToString()) && 
                    e.IN.Equals(identifier) &&
                    e.Enabled);
            if(employee == null)
            {
                return NotFound(ErrorType.GerenteNotFound.GetString());
            }
            //var gestores = employee.SubEmployees;
            var gestores = employee.Managers;
            return gestores.Count > 0 ?
                new JsonResult(gestores.Select(e => new EmpleadoDTO(e)))
                : NoContent();
        }
    }
}
