using System;
using System.Text.Json.Serialization;
using GestoresAPI.Models;

namespace GestoresAPI.DTO
{
    public class FacultyDTO
    {
        public FacultyDTO(Faculties faculty) =>
           (ID, Faculty, Icon, RegisteredAt) =
           (faculty.ID, faculty.Faculty, faculty.Icon, faculty.RegisteredAt);

        [JsonConstructor]
        public FacultyDTO()
        {
        }

        public int ID { get; set; }
        public string Faculty { get; set; }
        public string Icon { get; set; }
        public string RegisteredAt { get; set; }
    }
}
