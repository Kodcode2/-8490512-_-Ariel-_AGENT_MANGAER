﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using MosadAPIServer.DTO;
using MosadAPIServer.Enums;
using MosadAPIServer.ModelsHelpers;

namespace MosadAPIServer.Models
{
    public class Target : IModel, ILocationable
    {
        public Target(TargetDTO targetDTO)
        {
            Name = targetDTO.Name;
            Role = targetDTO.Position;
            PhotoUrl = targetDTO.PhotoUrl;
            
        }
        public Target() { }
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public int? LocationX { get; set; } = null;
        public int? LocationY { get; set; } = null;
       
        public string Name { get; set; }
       
        public string Role { get; set; }
       
        public TargetStatus Status { get; set; } = TargetStatus.Alive;
        public bool AssignedToMission { get; set; } = false;
        [JsonIgnore]
        public List<Mission>? Missions { get; set; }

        public Location? GetLocation()
        {
            if(LocationX == null || LocationY == null) return null;
            return new Location(LocationX , LocationY );
        }

        public void SetLocation(Location newLocation)
        {
            LocationX = newLocation.X;
            LocationY = newLocation.Y;
        }
    }
}
