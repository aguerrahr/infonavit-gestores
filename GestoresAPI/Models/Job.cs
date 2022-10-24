using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestoresAPI.Models
{
    [Table("Jobs")]
    public class Job
    {
        [Key]
        [Column("JobId")]
        public byte ID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("Enabled")]
        public bool Enabled { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Job> JobManagers { get; set; }
        public virtual ICollection<Job> SubJobs { get; set; }
        public virtual ICollection<Modules> Modules { get; set; }
        public virtual ICollection<Faculties> Faculties { get; set; }
    }
}
