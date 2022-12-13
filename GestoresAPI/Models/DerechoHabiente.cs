using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace GestoresAPI.Models
{
    [Table("DERECHO_HABIENTES")]
    [Index("Curp", "Nss", "TipoDerechoHabiente", Name = "IX_dh_unico_curp_nss", IsUnique = true)]
    public class DerechoHabiente
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("NOMBRE")]
        [StringLength(60)]        
        public string? Nombre { get; set; }
        [Column("A_PATERNO")]
        [StringLength(60)]        
        public string? APaterno { get; set; }
        [Column("A_MATERNO")]
        [StringLength(60)]        
        public string? AMaterno { get; set; }
        [Column("NSS")]
        [StringLength(11)]
        
        public string? Nss { get; set; }
        [Column("CURP")]
        [StringLength(18)]
        
        public string? Curp { get; set; }
        [Column("FH_ENROLAMIENTO", TypeName = "datetime")]
        public DateTime? FhEnrolamiento { get; set; }
        [Column("FH_MODIFICACION", TypeName = "datetime")]
        public DateTime? FhModificacion { get; set; }
        [Column("USUARIO_ENROLA")]
        [StringLength(11)]
        
        public string? UsuarioEnrola { get; set; }
        [Column("USUARIO_MODIFICA")]
        [StringLength(11)]
        
        public string? UsuarioModifica { get; set; }
        [Column("ACTIVO")]
        public int? Activo { get; set; }
        [Column("TIPO_DERECHO_HABIENTE")]
        public int TipoDerechoHabiente { get; set; }
    
        public virtual TipoDerechoHabiente TipoDerechoHabientes { get; set; }

    }
}
