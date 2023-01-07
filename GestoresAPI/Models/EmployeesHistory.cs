using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestoresAPI.Models
{
    
    [Table("EmployeesHistory")]    
    public partial class EmployeesHistory
    {
        [Key, Column("EmployeeId", Order = 1)]
        
        public string ID { get; set; }
        [Column("JobId")]
        public byte IdJob { get; set; }        
        [Column("In")]
        public string IN { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("LastName")]
        public string LastName { get; set; }
        [Column("MiddleName")]
        public string MiddleName { get; set; }
        [Column("PersonalNumber")]
        public string CURP { get; set; }
        [Column("PersonalNumber2")]
        public string RFC { get; set; }
        [Column("Ssn")]
        public string NSS { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("Enabled")]
        public bool Enabled { get; set; }
        [Column("Enrolled")]
        public byte Enrolled { get; set; }
        [Column("InRegistra")]
        [StringLength(15)]
        public string? InRegistra { get; set; }
        [Column("InModifica")]
        [StringLength(15)]
        public string? InModifica { get; set; }
        [Column("InOperativo")]
        [StringLength(15)]
        public string? InOperativo { get; set; }
        [Column("FacultyId")]
        public int FacultyId { get; set; }
    }
}
