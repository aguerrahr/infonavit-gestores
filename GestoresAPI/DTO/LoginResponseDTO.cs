using GestoresAPI.Models;
using System.Collections.Generic;

namespace GestoresAPI.DTO
{
    public class LoginResponseDTO
    {
        public LoginResponseDTO(EmpleadoDTO Employee, IEnumerable<ModuleDTO> Modules, IEnumerable<FacultyDTO> Faculties) {
            this.Employee = Employee;
            this.Modules = Modules;
            this.Faculties = Faculties;
        }
        public EmpleadoDTO Employee { get; set; }
        public IEnumerable<ModuleDTO> Modules { get; set; }
        public IEnumerable<FacultyDTO> Faculties { get; set; }

    }
}
