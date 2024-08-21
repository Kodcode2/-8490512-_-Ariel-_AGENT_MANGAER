using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MosadAPIServer.Enums;
using MosadAPIServer.SubModels;

namespace MosadAPIServer.Models
{
    public class Target 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public TargetStatus Status { get; set; }

        public List<Mission>? Missions { get; set; }
    }
}
