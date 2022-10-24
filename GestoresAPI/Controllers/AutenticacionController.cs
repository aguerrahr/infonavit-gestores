using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

using GestoresAPI.DTO;
using GestoresAPI.Models.Contexts;

[ApiController]
[Route("api")]
public class AutenticacionController : ControllerBase
{
    private readonly GestoresAPIContext context;
    private readonly ILogger<AutenticacionController> _logger;

        public AutenticacionController(GestoresAPIContext context, ILogger<AutenticacionController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login()
        {
            _logger.LogInformation("Login successful.");
            var identifier = HttpContext.Items["IN"];
            _logger.LogInformation("IN received: " + identifier);
            var employee = this.context.Employees.FirstOrDefault(e => e.ID.Equals(identifier));
            _logger.LogInformation("Employee's Job: " + employee.IdJob);
            var job = employee.Job;
            var faculties = job.Faculties;
            var modules = job.Modules;
        return new JsonResult(new LoginResponseDTO(
                new EmpleadoDTO(employee),
                modules == null || modules.Count() == 0
                    ? Enumerable.Empty<ModuleDTO>() 
                    : modules.Where(e => e.Enabled).Select(m => new ModuleDTO(m)),
                faculties == null || faculties.Count() == 0
                    ? Enumerable.Empty<FacultyDTO>()
                    : faculties.Where(e => e.Enabled).Select(f => new FacultyDTO(f))
                ));
        }

        [Route("Logout")]
        [HttpDelete]
        public void Logout()
        {
            _logger.LogInformation("Logout successful.");
        }
}
