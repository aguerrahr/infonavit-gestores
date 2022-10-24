using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationAPI.Models
{
    [Table("Authentication")]
    public class Authentication
    {
        [Key]
        [Column("EmployeeId")]
        public string ID { get; set; }
        public virtual Employee Employee { get; set; }
        [Column("In")]
        public string IN { get; set; }
        [Column("Attempts")]
        public Byte Attempts { get; set; }
        [Column("Token")]
        public string Token { get; set; }
        [Column("TmpToken")]
        public string TemporalToken { get; set; }
        [Column("Ip")]
        public string IP { get; set; }
        [Column("Enabled")]
        public bool Enabled { get; set; }
        [Column("AuthenticatedAt")]
        public DateTime AuthenticatedAt { get; set; }
    }
}
