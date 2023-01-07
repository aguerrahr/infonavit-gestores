using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AuthenticationAPI.Models
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        [Column("EmployeeId")]
        public string ID { get; set; }
        [Column("In")]
        public string IN { get; set; }
        [Column("Enabled")]
        public bool Enabled { get; set; }
        [Column("JobId")]
        public Byte IdJob { get; set; }        
        public virtual Job Job { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual Authentication Authentication { get; set; }
    }
}
