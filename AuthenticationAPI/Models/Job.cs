using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationAPI.Models
{
    [Table("Jobs")]
    public class Job
    {
        [Key]
        [Column("JobId")]
        public Byte ID { get; set; }
        [Column("Name")]
        public String Name { get; set; }
        [Column("Group")]
        public String Group { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("Enabled")]
        public bool Enabled { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
