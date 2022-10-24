using GestoresAPI.Models;
using System;
using System.Text.Json.Serialization;

namespace GestoresAPI.DTO
{
    public class SettingsDTO
    {
        public SettingsDTO(Settings setings) =>
          (Key, Value, Description, RegisteredAt) =
          (setings.Key, setings.Value, setings.Description, setings.CreatedAt);

        [JsonConstructor]
        public SettingsDTO()
        {
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
