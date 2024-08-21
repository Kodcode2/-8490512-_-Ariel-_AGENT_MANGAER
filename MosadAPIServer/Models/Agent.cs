using MosadAPIServer.Enums;
using MosadAPIServer.SubModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosadAPIServer.Models
{
    public class Agent 
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public string NickName { get; set; }
        public AgentStatus Status { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }

        List<Mission>? Missions { get; set; }
    }
}
