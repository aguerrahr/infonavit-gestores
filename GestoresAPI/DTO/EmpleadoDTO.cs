using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GestoresAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestoresAPI.DTO
{
    public class EmpleadoDTO
    {
        public EmpleadoDTO(Employee employee) =>
            (IN, Name, LastName, MiddleName, RFC, NSS, CreatedAt,UpdatedAt, CURP, IdJob, Enrolled, InRegistra, InModifica,NbRegistra, NbModifica) = 
            (employee.IN, employee.Name, employee.LastName, employee.MiddleName, employee.RFC, employee.NSS, employee.CreatedAt, employee.UpdatedAt, 
            employee.CURP, employee.IdJob, employee.Enrolled,employee.InRegistra, employee.InModifica, 
            employee.NbRegistra, employee.NbModifica);

        [JsonConstructor]
        public EmpleadoDTO()
        {
        }
          

        public string IN { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CURP { get; set; }
        public string RFC { get; set; }
        public string NSS { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;
        public byte IdJob { get; set; }
        public byte Enrolled { get; set; }

        public string InRegistra { get; set; }
        public string InModifica { get; set; }

        [NotMapped]
        public string NbRegistra { get; set; }
        [NotMapped]
        public string NbModifica { get; set; }
        

    }
}
