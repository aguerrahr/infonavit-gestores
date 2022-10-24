using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestoresAPI.Models
{
    [Table("Settings")]
    public class Settings
    {
        [Key]
        [Column("SettingId")]
        public int ID { get; set; }
        [Column("Key")]
        public string Key { get; set; }
        [Column("Value")]
        public string Value { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}
