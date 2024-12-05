using System;
using System.Text.Json.Serialization;

namespace Blip.Models
{
    public class Repositorio
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }      // Título (Nome do Repositório)

        [JsonPropertyName("description")]
        public string Description { get; set; } // Descrição do Repositório

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } // Data de Criação

        [JsonPropertyName("language")]
        public string Language { get; set; } // Linguagem do Repositório
    }
}
