using GestoresAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace GestoresAPI.DTO
{
    public class JobDTO
    {
        public JobDTO(JobDTO job) => (
            ID, Name, Description, CreatedAt, Enabled
            ) = (job.ID, job.Name, job.Description, job.CreatedAt, job.Enabled);

        [JsonConstructor]
        public JobDTO()
        {
        }
        public byte ID { get; set; }        
        public string Name { get; set; }        
        public string Description { get; set; }        
        public DateTime CreatedAt { get; set; }        
        public bool Enabled { get; set; }
    }
}
