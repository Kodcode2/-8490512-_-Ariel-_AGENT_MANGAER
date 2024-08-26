using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.DTO;
using MosadAPIServer.Models;
using MosadAPIServer.Services;

namespace MosadAPIServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController : CruController<Agent, AgentDTO>
    {

        private AgentService _agentService;
        public AgentsController(MosadAPIServerContext context, AgentService agentService)
            : base(context, agentService) { 
            _agentService = agentService;
        }

        // GET: /agentsWithMissionId
        [HttpGet("agentsWithMissionId")]
        public async Task<ActionResult<IEnumerable<AgentWithMissionIdDTO>>> GetAllAgentsWithMissionId()
        {
            try
            {
                var list = await _agentService.GetAllAgentsWithMissionId();
                return Ok(list);

            }
            catch (Exception ex) { return NotFound(); }

        }

       
    }
}
