using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;

using GestoresAPI.DTO;
using GestoresAPI.Models;
using GestoresAPI.Models.Contexts;
using System.Collections.Generic;

namespace GestoresAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly GestoresAPIContext context;

        public SettingsController(GestoresAPIContext context, ILogger<SettingsController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult QueryAll()
        {
            _logger.LogInformation("Query All Settings");
            var settings = this.context.Settings.Select(s => new SettingsDTO(s));
            if (settings == null || 0 == settings.Count())
            {
                _logger.LogError("No se encontraron datos en la tabla Settings.");
                return NoContent();
            }
            return new JsonResult(settings);
        }
    }
}
