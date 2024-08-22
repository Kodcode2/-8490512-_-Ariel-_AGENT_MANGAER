﻿using System.Text.Json.Serialization;

namespace MosadAPIServer.DTO
{
    public class IDTOModel
    {
        [JsonPropertyName("photo_url")]
        public string PhotoUrl { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
