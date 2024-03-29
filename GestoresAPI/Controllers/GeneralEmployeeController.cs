﻿using Microsoft.AspNetCore.Mvc;
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
    public class GeneralEmployeeController : ControllerBase
    {
        private readonly ILogger<GeneralEmployeeController> _logger;
        private readonly GestoresAPIContext context;

        public GeneralEmployeeController(GestoresAPIContext context, ILogger<GeneralEmployeeController> logger)
        {
            this.context = context;
            _logger = logger;
        }
        
        [HttpGet("{idjob}/profiles")]
        [Produces("application/json")]
        public IActionResult GetAllProfiles([FromRoute(Name = "idjob")] int identifier)
        {
            _logger.LogInformation("Query All Profiles idjob = " + identifier);
            
            var employees = this.context.Employees.Where(e =>e.IdJob.Equals(identifier)).ToList();

            var query = (
                    from a in context.Employees.Where(a => a.IdJob == identifier && a.Enabled == true)
                    select new EmpleadoDTO
                    {
                        IN = a.IN,
                        Name = a.Name,
                        LastName = a.LastName,
                        MiddleName = a.MiddleName,
                        CURP = a.CURP,
                        RFC = a.RFC,
                        NSS = a.NSS,
                        IdJob = a.IdJob,
                        Enrolled = a.Enrolled
                    }
                );

            var rows = query.ToList();
            return rows.Count > 0 ?
                new JsonResult(rows)
                : NoContent();
        }


        [HttpGet("{in}")]
        [Produces("application/json")]
        public IActionResult GetEmpleado([FromRoute(Name = "in")] string identifier)
        {
            _logger.LogInformation("Query All Profiles idjob = " + identifier);

            var employees = this.context.Employees.Where(e => e.IdJob.Equals(identifier)).ToList();

            var query = (
                    from a in context.Employees.Where(a => a.IN == identifier)
                    select new EmpleadoDTO
                    {
                        IN = a.IN,
                        Name = a.Name,
                        LastName = a.LastName,
                        MiddleName = a.MiddleName,
                        CURP = a.CURP,
                        RFC = a.RFC,
                        NSS = a.NSS,
                        IdJob = a.IdJob,
                        Enrolled = a.Enrolled
                    }
                );

            var rows = query.ToList();
            return rows.Count > 0 ?
                new JsonResult(rows)
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
                return Conflict("El registro ya se encuentra realizado.");
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
                verifyEmployee.IdJob = employeeRequest.IdJob;
                verifyEmployee.CreatedAt = DateTime.Now;
                this.context.SaveChanges();
                return new CreatedResult(
                new Uri("/GeneralEmployee/GetEmpleado/" + verifyEmployee.IN, UriKind.Relative),
                new EmpleadoDTO(verifyEmployee));
            }
            else
            {
                //var job = this.context.Jobs.FirstOrDefault(f => f.Name.Equals(JobNamesEnum.GERENTE.ToString()));
                var rolle = this.context.Roles.FirstOrDefault(a => a.ID == employeeRequest.IdJob);
                /*
                if (job == null)
                {
                    _logger.LogError("El Job PERFIL no ha sido dado de alta en la BDs.");
                    return NoContent();
                }
                if (rolle == null)
                {
                    _logger.LogError("El Rol no ha sido dado de alta en la BDs.");
                    return NoContent();
                }
                */
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
                    //IdJob = job.ID,
                    IdJob = employeeRequest.IdJob,
                    CreatedAt = DateTime.Now,
                    Roles = new List<Role>() { rolle }
                };
                this.context.Employees.Add(employee);
                this.context.SaveChanges();
                
                return new CreatedResult(
                    new Uri("/GeneralEmployee/GetEmpleado/" + employee.IN, UriKind.Relative),
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
            //this.context.Employees.Remove(employee);
            this.context.SaveChanges();
            return NoContent();
        }
    }
}
