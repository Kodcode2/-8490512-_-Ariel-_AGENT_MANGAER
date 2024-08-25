using MosadAPIServer.Enums;
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

        #region foreign_keys
        public int AgentId { get; set; }
        public Agent? Agent { get; set; }
        public int TargetId { get; set; }
        public Target? Target { get; set; }
        #endregion

        public DateTime? AssignedTime { get; set; } = null;
        public MissionStatus Status { get; set; } = MissionStatus.OpenForAssignment;
        public TimeSpan? Duration { get; set; }
        public TimeSpan? TimeToKill { get; set; }
        


    }
}
