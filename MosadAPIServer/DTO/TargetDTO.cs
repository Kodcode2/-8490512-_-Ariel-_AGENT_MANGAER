﻿using System.Text.Json.Serialization;

namespace MosadAPIServer.DTO
{
    public class TargetDTO : IDTOModel
    {
        public string Token { get; set; }
        [JsonPropertyName("photo_url")]
        public string PhotoUrl { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
    }
}
