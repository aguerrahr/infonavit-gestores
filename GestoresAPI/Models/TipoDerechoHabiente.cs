using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestoresAPI.Models
{
    [Table("TIPO_DERECHO_HABIENTE")]
    public class TipoDerechoHabiente
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("DESCRIPCION")]
        [StringLength(60)]
        public string? Descripcion { get; set; }
        public virtual ICollection<DerechoHabiente> DerechoHabientes { get; set; }
    }
}
