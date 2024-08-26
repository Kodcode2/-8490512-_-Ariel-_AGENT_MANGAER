using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Models;

namespace MosadAPIServer.Data
{
    public class MosadAPIServerContext : DbContext
    {
        public MosadAPIServerContext (DbContextOptions<MosadAPIServerContext> options)
            : base(options)
        {
        }

        public DbSet<MosadAPIServer.Models.Agent> Agent { get; set; } = default!;
        public DbSet<MosadAPIServer.Models.Target> Target { get; set; } = default!;
        public DbSet<MosadAPIServer.Models.Mission> Mission { get; set; } = default!;
    }
}
