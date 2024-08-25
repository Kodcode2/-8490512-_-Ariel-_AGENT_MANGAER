using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.DTO;
using MosadAPIServer.Models;
using MosadAPIServer.Services;
using MosadAPIServer.ModelsHelpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MosadAPIServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public abstract class CruController<T,DTO>  : ControllerBase where DTO  :IDTOModel where T : IModel
    {
        protected readonly MosadAPIServerContext _context;
        protected readonly ICruService<T,DTO> _ModelService;

        public CruController(MosadAPIServerContext context, ICruService<T, DTO> service)
        {
            _context = context;
            _ModelService = service;
        }

        // GET: /T
        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            return await _ModelService.GetAllAsync();
        }

        // POST: /T
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAgent(int id)
        {
            if (id == null) { return BadRequest("wrong id"); }

            var agent = _context.Agent.Find(id);
            if (agent == null) {
                return NotFound();
            }
            return Ok(agent);
        }


        // POST: /T
        [HttpPost("")]
        public async Task<ActionResult> PostAgent(DTO dtomodel)
        {
            if (!ModelState.IsValid) { return BadRequest("wrong body"); }

            int id = await _ModelService.CreateAsync(dtomodel);
            return Created(nameof(GetAgent), new { id = id });
        }

        // PUT: /T/5/pin
        [HttpPut("{id}/pin")]
        public async Task<IActionResult> PutAgent(int id, PinDTO pinLocation)
        {
            if (id == null || !_ModelService.IsExists(id)) return NotFound("worng id");
            try
            {
                await _ModelService.PinLocatinAsync(id, pinLocation);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_ModelService.IsExists(id))
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // PUT: /T/5/move
        [HttpPut("{id}/move")]
        public async Task<IActionResult> MoveAgent(int id, [FromBody] string direction)
        {
            if (!ModelState.IsValid) { return BadRequest("wrong body"); }

            if (id == null ||!_ModelService.IsExists(id)) return NotFound("worng id");

            try
            {
                await _ModelService.MoveAsync(id, direction);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_ModelService.IsExists(id))
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


       
    }
}

