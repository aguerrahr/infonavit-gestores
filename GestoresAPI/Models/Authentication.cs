using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace GestoresAPI.Models
{
    [Keyless]
    [Table("Authentication")]
    public class Authentication
    {
        [StringLength(15)]        
        public string EmployeeId { get; set; } = null!;
        [StringLength(15)]
        public string In { get; set; } = null!;
        public byte Attempts { get; set; }
        [StringLength(50)]
        public string? Token { get; set; }
        [StringLength(50)]
        public string? TmpToken { get; set; }
        [StringLength(15)]
        public string? Ip { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AuthenticatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
        public bool Enabled { get; set; }
    }
}
