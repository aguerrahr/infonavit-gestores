using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestoresAPI.Models
{
    [Table("Faculties")]
    public class Faculties
    {
        [Key]
        [Column("FacultyId")]
        public int ID { get; set; }
        [Column("Faculty")]
        public string Faculty { get; set; }
        [Column("Icon")]
        public string Icon { get; set; }
        [Column("RegisteredAt")]
        public string RegisteredAt { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("Enabled")]
        public bool Enabled { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
