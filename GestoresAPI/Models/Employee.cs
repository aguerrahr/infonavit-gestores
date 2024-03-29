﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace GestoresAPI.Models
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        [Column("EmployeeId")]
        public string ID  { get; set; }
        [Column("JobId")]
        public byte IdJob { get; set; }
        public virtual Job Job { get; set; }
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
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Employee> Managers { get; set; }
        public virtual ICollection<Employee> SubEmployees { get; set; }
    }
}
