using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationAPI.Models
{
    [Table("Resources")]
    public class Resource
    {
        [Key]
        [Column("ResourceId")]
        public short ID { get; set; }
        [Column("Path")]
        public String Path { get; set; }
        [Column("Enabled")]
        public bool Enabled { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
