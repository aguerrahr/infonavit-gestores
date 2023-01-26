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
    public class EmployeesHistoryController : ControllerBase
    {
        private readonly ILogger<EmployeesHistoryController> _logger;
        private readonly GestoresAPIContext context;

        public EmployeesHistoryController(GestoresAPIContext context, ILogger<EmployeesHistoryController> logger)
        {
            this.context = context;
            _logger = logger;
        }
        [HttpGet]
        [Produces("application/json")]
        public IActionResult QueryAll()
        {
            _logger.LogInformation("Query All EmployeesHistory");
            IQueryable<EmpleadoHistoriaDTO> employee =
                          (
                          from em in context.EmployeesHistories
                          select new EmpleadoHistoriaDTO
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
                              CreatedAt= em.CreatedAt,
                              UpdatedAt= em.UpdatedAt,
                              FacultyId= context.Faculties.Where(x=>x.ID == em.FacultyId).FirstOrDefault().Faculty,
                              NbJob = (
                                from j in context.Jobs
                                where (j.ID == em.IdJob)
                                select (j.Name)
                                ).FirstOrDefault().ToString()

                          }
                          ).OrderBy(x => x.IN);
            return employee != null ?
                    new JsonResult(employee)
                    : NoContent();
        }
        [HttpGet("GetRepAuthentication")]
        [Produces("application/json")]
        public IActionResult GetRepAuthentication()
        {
            _logger.LogInformation("Fetching Tipo derechohabiente");
            var query = (
                from tdh in context.Authentications
                select new EmpleadoHistoriaDTO
                {
                    IN = tdh.In,
                    Name = ((from b in context.Employees
                               where b.IN == tdh.In
                               select (b.Name + " " + b.LastName + " " + b.MiddleName)).FirstOrDefault().ToString()
                             ),
                   CreatedAt  = tdh.AuthenticatedAt
                }
            );
            var rows = query.ToList();
            return new JsonResult(rows);
        }
    }
}
