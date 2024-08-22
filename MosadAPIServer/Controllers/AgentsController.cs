using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.Models;
using MosadAPIServer.Services;

namespace MosadAPIServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly MosadAPIServerContext _context;
        private readonly AgentService _agentService;

        public AgentsController(MosadAPIServerContext context , AgentService agentService )
        {
            _context = context;
            _agentService = agentService;
        }

        // GET: api/Agents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAgent()
        {
            return await _context.Agent.ToListAsync();
        }

       


        // POST: api/Agents
        [HttpPost]
        public async Task<ActionResult> PostAgent([FromBody] string nickname , [FromBody] string photo_url)
        {
            var newAgent = new Agent() { NickName = nickname,PhotoUrl=photo_url};
            _context.Agent.Add(newAgent);
            await _context.SaveChangesAsync();
            return Created(nameof(PostAgent), new { id = newAgent.Id });
        }

        // PUT: api/Agents/5/pin
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> PutAgent(int id, Location pinLocation)
        {
            try
            {
                await _agentService.PinLocatinAsync(id, pinLocation);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (NullReferenceException ex) 
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/Agents/5/move
        [HttpPut("{id}/move")]
        public async Task<IActionResult> MoveAgent(int id , [FromBody]string direction)
        {
            try
            {
                await _agentService.MoveAsync(id, direction);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (NullReferenceException ex)
            {
                return NotFound();
            }

            return NoContent();
        }


        private bool AgentExists(int id)
        {
            return _context.Agent.Any(e => e.Id == id);
        }
    }
}
