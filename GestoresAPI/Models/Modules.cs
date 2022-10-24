using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestoresAPI.Models
{
    [Table("Modules")]
    public class Modules
    {
        [Key]
        [Column("ModuleId")]
        public int ID { get; set; }
        [Column("Module")]
        public string Module { get; set; }
        [Column("Icon")]
        public string Icon { get; set; }
        [Column("RegisteredAt")]
        public string RegisteredAt { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("Enabled")]
        public bool Enabled { get; set; }
        [Column("url")]
        public string Url { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
