using MosadAPIServer.Enums;
using MosadAPIServer.SubModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosadAPIServer.Models
{
    public class Mission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AgentId { get; set; }
        public Agent? Agent { get; set; }
        public int TargetId { get; set; }
        public Target? Target { get; set; }
        public DateTime? AssignedTime { get; set; }
        public MissionStatus Status { get; set; }
        public TimeSpan? Duration { get; set; }
        public TimeSpan? TimeToKill { get; set; }
        


    }
}
