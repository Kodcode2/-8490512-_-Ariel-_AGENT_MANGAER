﻿using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.DTO;
using MosadAPIServer.Models;
using MosadAPIServer.ModelsHelpers;

namespace MosadAPIServer.Services
{
    public class TargetService : ICruService<Target, TargetDTO>
    {
        private readonly MosadAPIServerContext _context;
        private readonly MissionService _missionService;

        public TargetService(MosadAPIServerContext context, MissionService missionService)
        {
            _context = context;
            _missionService = missionService;
        }
        public async Task<int> CreateAsync(TargetDTO targetDTO)
        {
            
            var newTarget = new Target(targetDTO);
            _context.Target.Add(newTarget);
            await _context.SaveChangesAsync();
            return newTarget.Id;
            
        }

        public async Task<List<Target>> GetAllAsync()
        {
            return await _context.Target.ToListAsync();
        }


        public async Task MoveAsync(int id, string dir)
        {
           var target = _context.Target.Find(id);

            if (target.Status == Enums.TargetStatus.Terminated)
                throw new InvalidOperationException("cannot move a teminated target");

            Location? newLocation = DirectionsService.Move(target.GetLocation(), dir);
            if (newLocation == null)
                throw new InvalidOperationException("cannot move beyond bounds");

            target.SetLocation(newLocation);
            _context.Entry(target).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            // no await
            _missionService.TargetMoved(target.Id);
        }

        public async Task PinLocatinAsync(int id, Location pinLocation)
        {
            var target = await _context.Target.FindAsync(id);

            if (target == null)
            {
                throw new NullReferenceException("target not found");
            }

            _context.Entry(target).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public bool IsExists(int id)
        {
            return _context.Target.Any(e => e.Id == id);
        }
    }
}
