using System;
using System.Text.Json.Serialization;
using GestoresAPI.Models;

namespace GestoresAPI.DTO
{
    public class ModuleDTO
    {
        public ModuleDTO(Modules module) =>
          (ID, Module, Icon, RegisteredAt, Url) =
          (module.ID, module.Module, module.Icon, module.RegisteredAt, module.Url);

        [JsonConstructor]
        public ModuleDTO()
        {
        }

        public int ID { get; set; }
        public string Module { get; set; }
        public string Icon { get; set; }
        public string RegisteredAt { get; set; }
        public string Url { get; set; }
    }
}
