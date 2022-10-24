using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestoresAPI.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [Column("RoleId")]
        public short ID { get; set; }
        [Column("Name")]
        public String Name { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("Enabled")]
        public bool Enabled { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
