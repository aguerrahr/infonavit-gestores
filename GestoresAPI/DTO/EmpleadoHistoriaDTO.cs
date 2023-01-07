using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Text.Json.Serialization;
using GestoresAPI.Models;

namespace GestoresAPI.DTO
{
    public class EmpleadoHistoriaDTO
    {
        public EmpleadoHistoriaDTO(EmployeesHistory employee) =>
            (IN, Name, LastName, MiddleName, RFC, NSS, CURP, IdJob, Enrolled, InRegistra, InModifica, InOperativo) =
            (employee.ID, employee.Name, employee.LastName, employee.MiddleName, employee.RFC, employee.NSS, employee.CURP, employee.IdJob, employee.Enrolled, employee.InRegistra, employee.InModifica, employee.InOperativo);

        [JsonConstructor]
        public EmpleadoHistoriaDTO()
        {
        }
        public string ID { get; set; }
        public byte IdJob { get; set; }
        public virtual Job Job { get; set; }
        public string IN { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CURP { get; set; }
        public string RFC { get; set; }
        public string NSS { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Enabled { get; set; }
        public byte Enrolled { get; set; }
        public string? InRegistra { get; set; }
        public string? InModifica { get; set; }
        public string? InOperativo { get; set; }
        public int FacultyId { get; set; }
    }
}
