using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.Models;
using MosadAPIServer.Services;

namespace MosadAPIServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController : ControllerBase
    {
        private readonly MosadAPIServerContext _context;
        private readonly MissionService _missionService;

        public MissionsController(MosadAPIServerContext context, MissionService missionService)
        {
            _context = context;
            _missionService = missionService;
        }

        // GET: Missions
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Mission>>> GetMission(string? status)
        {
            return Ok(await _missionService.getAllMissions(status));
        }

        // GET: Missions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mission>> GetMission(int id)
        {
            var mission = await _context.Mission.FindAsync(id);

            if (mission == null)
            {
                return NotFound();
            }

            return mission;
        }

        // PUT: Missions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMission(int id, Mission mission)
        {
            if (id != mission.Id)
            {
                return BadRequest();
            }

            if (!MissionExists(id))
            {
                return NotFound();
            }

            try
            {
                await _missionService.AssignMission(mission);//_context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MissionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: Missions/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateMissions()
        {
            _missionService.UpdateMissions();
            return Ok();
        }




        private bool MissionExists(int id)
        {
            return _context.Mission.Any(e => e.Id == id);
        }
    }
}
