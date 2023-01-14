using GestoresAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace GestoresAPI.DTO
{
     public class DerechoHabienteDTO
    {
        public DerechoHabienteDTO(DerechoHabiente derechoHabiente) =>
          (Id, Nombre, APaterno, AMaterno, Nss, Curp, FhEnrolamiento, FhModificacion, UsuarioEnrola, UsuarioModifica, Activo, TipoDerechoHabiente) =
(derechoHabiente.Id, derechoHabiente.Nombre, derechoHabiente.APaterno, derechoHabiente.AMaterno, derechoHabiente.Nss, derechoHabiente.Curp, 
            derechoHabiente.FhEnrolamiento, derechoHabiente.FhModificacion, derechoHabiente.UsuarioEnrola, derechoHabiente.UsuarioModifica, 
            derechoHabiente.Activo, derechoHabiente.TipoDerechoHabiente);

        [JsonConstructor]
        public DerechoHabienteDTO()
        {
        }
        
        public int Id { get; set; }               
        public string? Nombre { get; set; }        
        public string? APaterno { get; set; }
        public string? AMaterno { get; set; }
        public string? Nss { get; set; }
        public string? Curp { get; set; }
        public DateTime? FhEnrolamiento { get; set; }
        public DateTime? FhModificacion { get; set; }
        public string? UsuarioEnrola { get; set; }
        public string? UsuarioModifica { get; set; }
        public int? Activo { get; set; }
        public int TipoDerechoHabiente { get; set; }
        public String NbTpDH { get; set; }
        public String NbUsuairoEnrola { get; set; }
        public String NbUsuairoModifica { get; set; }
        public String InUsuairoEnrola { get; set; }
        public String InUsuairoModifica { get; set; }
    }    
}

